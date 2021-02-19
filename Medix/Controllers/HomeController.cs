using Medix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Medix.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string sortOrder, int? page)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWorkOrders()
        {
            List<WorkOrderModel> lstModel = new List<WorkOrderModel>();
            using (MedixEntities entities = new MedixEntities())
            {
                var lstEntities = entities.Sp_GetWorkOrders().OrderBy(x => x.WorkOrderId).ToList();
                foreach(var entity in lstEntities)
                {
                    var model = new WorkOrderModel();
                    model.WorkOrderID = entity.WorkOrderId;
                    model.WorkOrderNumber = entity.WorkOrderNumber;
                    model.ClientID = entity.ClientId;
                    model.ClientName = entity.ClientName;
                    model.StateID = entity.StateId;
                    model.StatusID = entity.StatusId;
                    model.StatusName = entity.StatusName;
                    model.ETA = entity.ETA == null ? "" : entity.ETA.ToString();
                    model.CreatedDate = entity.CreatedDate == null ? "" : entity.CreatedDate.ToString();
                    lstModel.Add(model);
                }
            }

                
            return Json(lstModel.OrderBy(x=>x.WorkOrderID), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ClientResource()
        {
            MedixEntities entities = new MedixEntities();
            return Json(entities.Sp_GetClients().ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StateResource()
        {
            MedixEntities entities = new MedixEntities();
            return Json(entities.Sp_GetStates().ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StatusResource()
        {
            MedixEntities entities = new MedixEntities();
            return Json(entities.Sp_GetStatus().ToList(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult WorkOrderCount()
        {
            MedixEntities entities = new MedixEntities();
            var cnt = entities.Sp_GetWorkOrderCount().ToList().First();
            
            return Json(cnt == null? 0: (int)cnt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetbyId(int id)
        {
            WorkOrderModel model = new WorkOrderModel();
            using (MedixEntities entities = new MedixEntities())
            {                
                var workItem = entities.Sp_GetWorkOrderItem(id).ToList().First();
                model.WorkOrderID = id;
                model.ClientID = workItem.ClientId;
                model.StateID = workItem.StateId;
                model.StatusID = workItem.StatusId;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddWorkOrder(FormCollection formdata)
        {
            var cid = formdata["client"];
            var stateid = formdata["state"];
            var statusid = formdata["status"];
            Sp_GetWorkOrders_Result workItem = new Sp_GetWorkOrders_Result();

            using (MedixEntities entities = new MedixEntities())
            {
                var index = 0;
                try
                {
                    index = entities.Sp_InsertWorkOrder(DateTime.Now.Ticks.ToString(), Convert.ToInt32(cid), Convert.ToInt32(stateid), Convert.ToInt32(statusid), DateTime.Now);
                    if (index == 0)
                    {
                        throw (new Exception("It is failed to create in WorkOrder table. Please retry later."));
                    }
                    else
                    {
                        workItem = entities.Sp_GetWorkOrders().OrderByDescending(x => x.WorkOrderId).ToList().First();
                    }
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            
            return Json(workItem, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateWorkOrder(FormCollection formdata)
        {
            var cid = formdata["client"];
            var stateid = formdata["state"];
            var statusid = formdata["status"];
            var orderid = formdata["orderid"];
            Sp_GetWorkOrderItem_Result updateItem = new Sp_GetWorkOrderItem_Result();
            WorkOrderModel model = new WorkOrderModel();

            using (MedixEntities entities = new MedixEntities())
            {
                var index = 0;
                try
                {
                    index = entities.Sp_UpdateWorkOrderOrder(Convert.ToInt32(orderid), Convert.ToInt32(cid), Convert.ToInt32(stateid), Convert.ToInt32(statusid), DateTime.Now);
                    if (index == 0)
                    {
                        throw (new Exception("It is failed to update in WorkOrder table. Please retry later."));
                    }
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                updateItem= entities.Sp_GetWorkOrderItem(Convert.ToInt32(orderid)).ToList().First();
                model.WorkOrderID = updateItem.WorkOrderId;
                model.WorkOrderNumber = updateItem.WorkOrderNumber;
                model.ClientID = updateItem.ClientId;
                model.ClientName = updateItem.ClientName;
                model.StateID = updateItem.StateId;
                model.StatusID = updateItem.StatusId;
                model.StatusName = updateItem.StatusName;
                model.ETA = updateItem.ETA == null ? "" : updateItem.ETA.ToString();
                model.CreatedDate = updateItem.CreatedDate == null ? "" : updateItem.CreatedDate.ToString();
            }

            return Json(updateItem, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteWorkOrder(int id)
        {
            using (MedixEntities entities = new MedixEntities())
            {
                try
                {
                    var res = entities.Sp_DeleteWorkOrderOrder(id);
                    if (res == 0)
                    {
                        throw (new Exception("It is failed to delete in WorkOrder table. Please retry later."));
                    }
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(e.Message, JsonRequestBehavior.AllowGet);
                }
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}