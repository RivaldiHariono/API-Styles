using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Loyalty.WebAPI.Models;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.IO;
using Microsoft.Crm.Sdk.Messages;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Loyalty.WebAPI.Controllers
{
    [RoutePrefix("api/discount")]
    public class DiscountsController : ApiController
    {
        [Route("Create")]
        public CreateDiscountResponse PostCreateDiscount(CreateDiscountRequest _request)
        {
            String urlorg = ConfigurationManager.AppSettings["CRMUrlOrg"].ToString();
            String organization = ConfigurationManager.AppSettings["CRMOrganization"].ToString();
            String domain = ConfigurationManager.AppSettings["CRMDomain"].ToString();
            String username = ConfigurationManager.AppSettings["CRMUsername"].ToString();
            String password = ConfigurationManager.AppSettings["CRMPassword"].ToString();
            String hostimage = ConfigurationManager.AppSettings["HostImage"].ToString();
            String hostimage2 = ConfigurationManager.AppSettings["HostImage2"].ToString();
            String curfileimage = ConfigurationManager.AppSettings["CurfileImage"].ToString();

            CRMLibs crmlibs = new CRMLibs(urlorg, organization, domain, username, password);
            CreateDiscountResponse response = new CreateDiscountResponse();

            try
            {
                Entity discount = new Entity("xrm_discounts");
                if(_request.DiscountName != null)
                    discount.Attributes["xrm_name"] = _request.DiscountName;
                if (_request.StartDate != null)
                    discount.Attributes["xrm_startdate"] = _request.StartDate;
                if (_request.EndDate != null)
                    discount.Attributes["xrm_enddate"] = _request.EndDate;
                if (_request.Description != null)
                    discount.Attributes["xrm_description"] = _request.Description;
                if (_request.StoreCategory != null)
                    discount.Attributes["xrm_storecategory"] = new EntityReference("xrm_storecategory", new Guid(_request.StoreCategory));
                if (_request.Mall != null)
                    discount.Attributes["xrm_mall"] = new EntityReference("xrm_mall", new Guid(_request.Mall));
                if (_request.Tag != null)
                    discount.Attributes["xrm_tag"] = _request.Tag;
                if (_request.Tenant != null)
                    discount.Attributes["xrm_tenant"] = new EntityReference("xrm_tenant", new Guid(_request.Tenant));

                Guid DiscountID = crmlibs.createentity(discount);
                Entity discountupdate = new Entity("xrm_discounts", DiscountID);

                if (_request.ImageBase64 != null && _request.ImageType != null)
                {
                    string path = Directory.GetCurrentDirectory();
                    string target = hostimage + @"Discount\Full\";
                    if (!Directory.Exists(target))
                    {
                        Directory.CreateDirectory(target);
                    }
                    Environment.CurrentDirectory = (target);
                    string target2 = hostimage2 + @"Discount\Full\";
                    if (!Directory.Exists(target2))
                    {
                        Directory.CreateDirectory(target2);
                    }
                    Environment.CurrentDirectory = (target2);
                    string imageName = DiscountID.ToString() + "-Full-" + DateTime.Now.ToString("yyyyMMdd") +"."+_request.ImageType;
                    string imgPath = Path.Combine(target, imageName);
                    string imgPath2 = Path.Combine(target2, imageName);
                    byte[] imageBytes = Convert.FromBase64String(_request.ImageBase64);
                    string curFile = curfileimage + @"Discount\Full\" + imageName;
                    File.WriteAllBytes(imgPath, imageBytes);
                    File.WriteAllBytes(imgPath2, imageBytes);

                    discountupdate.Attributes["xrm_imageurl"] = curFile;
                }

                crmlibs.updateentity(discountupdate);
                response.DiscountID = DiscountID.ToString();
                response.Code = "0";
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Code = "1";
                response.Message = ex.Message;
            }
            return response;
        }

        [Route("Update")]
        public UpdateDiscountResponse PostUpdateDiscount(UpdateDiscountRequest _request)
        {
            String urlorg = ConfigurationManager.AppSettings["CRMUrlOrg"].ToString();
            String organization = ConfigurationManager.AppSettings["CRMOrganization"].ToString();
            String domain = ConfigurationManager.AppSettings["CRMDomain"].ToString();
            String username = ConfigurationManager.AppSettings["CRMUsername"].ToString();
            String password = ConfigurationManager.AppSettings["CRMPassword"].ToString();
            String hostimage = ConfigurationManager.AppSettings["HostImage"].ToString();
            String hostimage2 = ConfigurationManager.AppSettings["HostImage2"].ToString();
            String curfileimage = ConfigurationManager.AppSettings["CurfileImage"].ToString();

            CRMLibs crmlibs = new CRMLibs(urlorg, organization, domain, username, password);
            UpdateDiscountResponse response = new UpdateDiscountResponse();

            try
            {
                EntityCollection discounts = crmlibs.getallrecord_byattribute("xrm_discounts", "xrm_discountsid", _request.DiscountID, new ColumnSet(new String[] { "xrm_name" }));
                if (discounts != null)
                    if(discounts.Entities.Count > 0)
                    {
                        Entity discount = discounts.Entities[0];
                        Guid discountid = discount.Id;

                        Entity updatediscount = new Entity("xrm_discounts", discountid);
                        if (_request.DiscountName != null)
                            updatediscount.Attributes["xrm_name"] = _request.DiscountName;
                        if (_request.StartDate != null)
                            updatediscount.Attributes["xrm_startdate"] = _request.StartDate;
                        if (_request.EndDate != null)
                            updatediscount.Attributes["xrm_enddate"] = _request.EndDate;
                        if (_request.StoreCategory != null)
                            updatediscount.Attributes["xrm_storecategory"] = new EntityReference("xrm_storecategory", new Guid(_request.StoreCategory));
                        if (_request.Mall != null)
                            updatediscount.Attributes["xrm_mall"] = new EntityReference("xrm_mall", new Guid(_request.Mall));
                        if (_request.Tag != null)
                            updatediscount.Attributes["xrm_tag"] = _request.Tag;
                        if (_request.Tenant != null)
                            updatediscount.Attributes["xrm_tenant"] = new EntityReference("xrm_tenant", new Guid(_request.Tenant));

                        if(_request.ImageBase64 != null && _request.ImageType !=null)
                        {
                            string path = Directory.GetCurrentDirectory();
                            string target = hostimage + @"Discount\Full\";
                            if (!Directory.Exists(target))
                            {
                                Directory.CreateDirectory(target);
                            }
                            Environment.CurrentDirectory = (target);
                            string target2 = hostimage2 + @"Discount\Full\";
                            if (!Directory.Exists(target2))
                            {
                                Directory.CreateDirectory(target2);
                            }
                            Environment.CurrentDirectory = (target2);
                            string imageName = discountid.ToString() + "-Full-" + DateTime.Now.ToString("yyyyMMdd") +"."+ _request.ImageType;
                            string imgPath = Path.Combine(target, imageName);
                            string imgPath2 = Path.Combine(target2, imageName);
                            byte[] imageBytes = Convert.FromBase64String(_request.ImageBase64);
                            string curFile = curfileimage + @"Discount\Full\" + imageName;
                            File.WriteAllBytes(imgPath, imageBytes);
                            File.WriteAllBytes(imgPath2, imageBytes);

                            updatediscount.Attributes["xrm_imageurl"] = curFile;
                        }
                        crmlibs.updateentity(updatediscount);
                        response.Code = "0";
                        response.Message = "Updated";
                    }
                    else
                    {
                        response.Code = "1";
                        response.Message = "Data not found";
                    }
            }
            catch(Exception ex)
            {
                response.Code = "1";
                response.Message = ex.Message;
            }
            return response;
        }

        [Route("List")]
        public ListDiscountResponse PostListDiscount()
        {
            String urlorg = ConfigurationManager.AppSettings["CRMUrlOrg"].ToString();
            String organization = ConfigurationManager.AppSettings["CRMOrganization"].ToString();
            String domain = ConfigurationManager.AppSettings["CRMDomain"].ToString();
            String username = ConfigurationManager.AppSettings["CRMUsername"].ToString();
            String password = ConfigurationManager.AppSettings["CRMPassword"].ToString();
            String hostimage = ConfigurationManager.AppSettings["HostImage"].ToString();
            String hostimage2 = ConfigurationManager.AppSettings["HostImage2"].ToString();
            String curfileimage = ConfigurationManager.AppSettings["CurfileImage"].ToString();

            CRMLibs crmlibs = new CRMLibs(urlorg, organization, domain, username, password);
            ListDiscountResponse response = new ListDiscountResponse();
            string q_discount = "";

            try
            {
                q_discount =
                    @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='xrm_discounts'>
                        <attribute name='xrm_discountsid' />
                        <attribute name='xrm_name' />
                        <attribute name='xrm_description' />
                        <attribute name='xrm_startdate' />
                        <attribute name='xrm_enddate' />
                        <attribute name='xrm_imageurl' />
                        <attribute name='xrm_mall' />
                        <attribute name='xrm_tenant' />
                        <attribute name='xrm_storecategory' />
                        <attribute name='xrm_tag' />
                        <order attribute='xrm_name' descending='false' />
                        <filter type='and'>
                          <condition attribute='statecode' operator='eq' value='0' />
                        </filter>
                      </entity>
                    </fetch>";

                EntityCollection discount = crmlibs.getallrecord_byexpression(q_discount);
                if (discount != null)
                {
                    if (discount.Entities.Count > 0)
                    {
                        response.ListDiscount = new List<ListDiscount>();
                        foreach (Entity Discount in discount.Entities)
                        {
                            ListDiscount listdiscount = new ListDiscount();
                            listdiscount.DiscountID = Discount.Id.ToString();

                            if (Discount.Attributes.Contains("xrm_name"))
                                listdiscount.DiscountName = Discount.Attributes["xrm_name"].ToString();
                            if (Discount.Attributes.Contains("xrm_imageurl"))
                                listdiscount.ImageType = Discount.Attributes["xrm_imageurl"].ToString();
                            if (Discount.Attributes.Contains("xrm_startdate"))
                                listdiscount.StartDate = ((DateTime)Discount.Attributes["xrm_startdate"]).ToLocalTime();
                            if (Discount.Attributes.Contains("xrm_enddate"))
                                listdiscount.EndDate = ((DateTime)Discount.Attributes["xrm_enddate"]).ToLocalTime();
                            if (Discount.Attributes.Contains("xrm_storecategory"))
                            {
                                EntityReference storecategory_ref = (EntityReference)Discount.Attributes["xrm_storecategory"];
                                listdiscount.StoreCategoryID = storecategory_ref.Id.ToString();
                                listdiscount.StoreCategoryName = storecategory_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_tenant"))
                            {
                                EntityReference tenant_ref = (EntityReference)Discount.Attributes["xrm_tenant"];
                                listdiscount.TenantID = tenant_ref.Id.ToString();
                                listdiscount.TenantName = tenant_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_mall"))
                            {
                                EntityReference mall_ref = (EntityReference)Discount.Attributes["xrm_mall"];
                                listdiscount.MallID = mall_ref.Id.ToString();
                                listdiscount.MallName = mall_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_tag"))
                                listdiscount.Tag = Discount.Attributes["xrm_tag"].ToString();

                            response.ListDiscount.Add(listdiscount);
                            response.Code = "0";
                            response.Message = "Success";

                        }
                    }
                    else
                    {
                        response.Code = "1";
                        response.Message = "No Data";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = "1";
                response.Message = ex.Message;
            }
            return response;
        }

        [Route("ListFilter")]
        public ListDiscountResponse PostListFilterDiscount(ListDiscountRequest _request)
        {
            String urlorg = ConfigurationManager.AppSettings["CRMUrlOrg"].ToString();
            String organization = ConfigurationManager.AppSettings["CRMOrganization"].ToString();
            String domain = ConfigurationManager.AppSettings["CRMDomain"].ToString();
            String username = ConfigurationManager.AppSettings["CRMUsername"].ToString();
            String password = ConfigurationManager.AppSettings["CRMPassword"].ToString();
            String hostimage = ConfigurationManager.AppSettings["HostImage"].ToString();
            String hostimage2 = ConfigurationManager.AppSettings["HostImage2"].ToString();
            String curfileimage = ConfigurationManager.AppSettings["CurfileImage"].ToString();

            CRMLibs crmlibs = new CRMLibs(urlorg, organization, domain, username, password);
            ListDiscountResponse response = new ListDiscountResponse();
            string q_discount = "";

            try
            {
                if(_request.StoreCategory != null)
                {
                    if(_request.Mall != null)
                    {
                        q_discount =
                        @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='xrm_discounts'>
                            <attribute name='xrm_discountsid' />
                            <attribute name='xrm_name' />
                            <attribute name='xrm_description' />
                            <attribute name='xrm_startdate' />
                            <attribute name='xrm_enddate' />
                            <attribute name='xrm_imageurl' />
                            <attribute name='xrm_mall' />
                            <attribute name='xrm_tenant' />
                            <attribute name='xrm_storecategory' />
                            <attribute name='xrm_tag' />
                            <order attribute='xrm_name' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                              <condition attribute='xrm_storecategory' operator='eq' value='" + _request.StoreCategory + @"'/>
                              <condition attribute='xrm_mall' operator='eq' value='" + _request.Mall + @"'/>
                            </filter>
                          </entity>
                        </fetch>";
                    }

                    else
                    {
                        q_discount =
                        @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='xrm_discounts'>
                            <attribute name='xrm_discountsid' />
                            <attribute name='xrm_name' />
                            <attribute name='xrm_description' />
                            <attribute name='xrm_startdate' />
                            <attribute name='xrm_enddate' />
                            <attribute name='xrm_imageurl' />
                            <attribute name='xrm_mall' />
                            <attribute name='xrm_tenant' />
                            <attribute name='xrm_storecategory' />
                            <attribute name='xrm_tag' />
                            <order attribute='xrm_name' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                              <condition attribute='xrm_storecategory' operator='eq' value='" + _request.StoreCategory + @"'/>
                            </filter>
                          </entity>
                        </fetch>";
                    }
                }
                else
                {
                    q_discount =
                        @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='xrm_discounts'>
                            <attribute name='xrm_discountsid' />
                            <attribute name='xrm_name' />
                            <attribute name='xrm_description' />
                            <attribute name='xrm_startdate' />
                            <attribute name='xrm_enddate' />
                            <attribute name='xrm_imageurl' />
                            <attribute name='xrm_mall' />
                            <attribute name='xrm_tenant' />
                            <attribute name='xrm_storecategory' />
                            <attribute name='xrm_tag' />
                            <order attribute='xrm_name' descending='false' />
                            <filter type='and'>
                              <condition attribute='statecode' operator='eq' value='0' />
                              <condition attribute='xrm_mall' operator='eq' value='" + _request.Mall + @"'/>
                            </filter>
                          </entity>
                        </fetch>";
                }

                EntityCollection discount = crmlibs.getallrecord_byexpression(q_discount);
                if (discount != null)
                {
                    if (discount.Entities.Count > 0)
                    {
                        response.ListDiscount = new List<ListDiscount>();
                        foreach (Entity Discount in discount.Entities)
                        {
                            ListDiscount listdiscount = new ListDiscount();
                            listdiscount.DiscountID = Discount.Id.ToString();

                            if (Discount.Attributes.Contains("xrm_name"))
                                listdiscount.DiscountName = Discount.Attributes["xrm_name"].ToString();

                            if (Discount.Attributes.Contains("xrm_imageurl"))
                                listdiscount.ImageBase64 = Discount.Attributes["xrm_imageurl"].ToString();

                            if (Discount.Attributes.Contains("xrm_startdate"))
                                listdiscount.StartDate = ((DateTime)Discount.Attributes["xrm_startdate"]).ToLocalTime();

                            if (Discount.Attributes.Contains("xrm_enddate"))
                                listdiscount.EndDate = ((DateTime)Discount.Attributes["xrm_enddate"]).ToLocalTime();

                            if (Discount.Attributes.Contains("xrm_storecategory"))
                            {
                                EntityReference storecategory_ref = (EntityReference)Discount.Attributes["xrm_storecategory"];
                                listdiscount.StoreCategoryID = storecategory_ref.Id.ToString();
                                listdiscount.StoreCategoryName = storecategory_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_tenant"))
                            {
                                EntityReference tenant_ref = (EntityReference)Discount.Attributes["xrm_tenant"];
                                listdiscount.TenantID = tenant_ref.Id.ToString();
                                listdiscount.TenantName = tenant_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_mall"))
                            {
                                EntityReference mall_ref = (EntityReference)Discount.Attributes["xrm_mall"];
                                listdiscount.MallID = mall_ref.Id.ToString();
                                listdiscount.MallName = mall_ref.Name;
                            }
                            if (Discount.Attributes.Contains("xrm_tag"))
                                listdiscount.Tag = Discount.Attributes["xrm_tag"].ToString();

                            if (Discount.Attributes.Contains("xrm_description"))
                                listdiscount.Description = Discount.Attributes["xrm_description"].ToString();

                            response.ListDiscount.Add(listdiscount);
                            response.Code = "0";
                            response.Message = "Success";
                        }
                    }
                    else
                    {
                        response.Code = "1";
                        response.Message = "No Data";
                    }

                }
            }
            catch (Exception ex)
            {
                response.Code = "1";
                response.Message = ex.Message;
            }
            return response;
        }

        [Route("Detail")]
        public DetailDiscountResponse PostDetailDiscount(DetailDiscountRequest _request)
        {
            String urlorg = ConfigurationManager.AppSettings["CRMUrlOrg"].ToString();
            String organization = ConfigurationManager.AppSettings["CRMOrganization"].ToString();
            String domain = ConfigurationManager.AppSettings["CRMDomain"].ToString();
            String username = ConfigurationManager.AppSettings["CRMUsername"].ToString();
            String password = ConfigurationManager.AppSettings["CRMPassword"].ToString();
            String hostimage = ConfigurationManager.AppSettings["HostImage"].ToString();
            String hostimage2 = ConfigurationManager.AppSettings["HostImage2"].ToString();
            String curfileimage = ConfigurationManager.AppSettings["CurfileImage"].ToString();

            CRMLibs crmlibs = new CRMLibs(urlorg, organization, domain, username, password);
            DetailDiscountResponse response = new DetailDiscountResponse();

            String q_discount = "";
            if (_request.DiscountID != null)
            {
                q_discount =
                    @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='xrm_discounts'>
                        <attribute name='xrm_discountsid' />
                        <attribute name='xrm_name' />
                        <attribute name='xrm_description' />
                        <attribute name='xrm_startdate' />
                        <attribute name='xrm_enddate' />
                        <attribute name='xrm_imageurl' />
                        <attribute name='xrm_mall' />
                        <attribute name='xrm_tenant' />
                        <attribute name='xrm_storecategory' />
                        <attribute name='xrm_tag' />
                        <order attribute='xrm_name' descending='false' />
                        <filter type='and'>
                            <condition attribute='statecode' operator='eq' value='0' />
                            <condition attribute='xrm_discountsid' operator='eq' value='" + _request.DiscountID + @"'/>
                        </filter>
                        </entity>
                    </fetch>";

                EntityCollection discount = crmlibs.getallrecord_byexpression(q_discount);
                if (discount != null)
                {
                    if (discount.Entities.Count > 0)
                    {
                        DetailDiscount DiscountData = new DetailDiscount();
                        Entity Discount = discount.Entities[0];

                        DiscountData.DiscountID = Discount.Id.ToString();

                        if (Discount.Attributes.Contains("xrm_name"))
                            DiscountData.DiscountName = Discount.Attributes["xrm_name"].ToString();
                        else
                            DiscountData.DiscountName = "";

                        if (Discount.Attributes.Contains("xrm_imageurl"))
                            DiscountData.ImageType = Discount.Attributes["xrm_imageurl"].ToString();
                        else
                            DiscountData.ImageType = "";

                        if (Discount.Attributes.Contains("xrm_startdate"))
                            DiscountData.StartDate = ((DateTime)Discount.Attributes["xrm_startdate"]).ToLocalTime();

                        if (Discount.Attributes.Contains("xrm_enddate"))
                            DiscountData.StartDate = ((DateTime)Discount.Attributes["xrm_enddate"]).ToLocalTime();

                        if (Discount.Attributes.Contains("xrm_storecategory"))
                        {
                            EntityReference storecategory_ref = (EntityReference)Discount.Attributes["xrm_storecategory"];
                            DiscountData.StoreCategoryID = storecategory_ref.Id.ToString();
                            DiscountData.StoreCategoryName = storecategory_ref.Name;
                        }
                        else
                        {
                            DiscountData.StoreCategoryID = "";
                            DiscountData.StoreCategoryName = "";
                        }

                        if (Discount.Attributes.Contains("xrm_tenant"))
                        {
                            EntityReference tenant_ref = (EntityReference)Discount.Attributes["xrm_tenant"];
                            DiscountData.TenantID = tenant_ref.Id.ToString();
                            DiscountData.TenantName = tenant_ref.Name;
                        }
                        else
                        {
                            DiscountData.TenantID = "";
                            DiscountData.TenantName = "";
                        }

                        if (Discount.Attributes.Contains("xrm_mall"))
                        {
                            EntityReference mall_ref = (EntityReference)Discount.Attributes["xrm_mall"];
                            DiscountData.MallID = mall_ref.Id.ToString();
                            DiscountData.MallName = mall_ref.Name;
                        }
                        else
                        {
                            DiscountData.MallID = "";
                            DiscountData.MallName = "";
                        }

                        if (Discount.Attributes.Contains("xrm_tag"))
                            DiscountData.Tag = Discount.Attributes["xrm_tag"].ToString();
                        else
                            DiscountData.Tag = "";

                        if (Discount.Attributes.Contains("xrm_description"))
                            DiscountData.Description = Discount.Attributes["xrm_description"].ToString();
                        else
                            DiscountData.Description = "";

                        response.DetailDiscount = DiscountData;
                        response.Code = "0";
                        response.Message = "Success";
                    }
                }
            }
            else
            {
                response.Code = "1";
                response.Message = "ID Can't be null";
            }
            return response;
        }

    }
}
