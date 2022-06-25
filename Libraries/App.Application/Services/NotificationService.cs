using App.Application.Framwork;
using App.Application.Interfaces;
using App.Application.Model;
using App.Application.ViewModels;
using App.Domain.Interfaces;
using App.Domain.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Services
{
    public  class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationReopsitory;
        private readonly IMapper _mapper;
        public NotificationService(INotificationRepository notificationRepository , IMapper mapper)
        {
            _notificationReopsitory = notificationRepository;
            _mapper = mapper;
        }

        public async Task<Tuple<IList<NotificationViewModel>, int>> LoadNotifcation( int AllListCount, int jtStartIndex = 0, int jtPageSize = 10, string CurrentCustomerId = null, bool? IsRead = null, string Search = null, string order = null, string orderDir = null)
        {
            AllListCount = 0;
            int totalRecords = 0;
            var data = await _notificationReopsitory.LoadItemsData(totalRecords, jtStartIndex, jtPageSize, CurrentCustomerId, IsRead, Search, order, orderDir);
            AllListCount = totalRecords;


            var list =  data.Item1.Select(obj =>
            {
                var model = _mapper.Map<NotificationViewModel>(obj);

                if (obj.CreateDate != null)
                {
                    model.MessageBody = obj.MessageBody;
                    model.TotalRecored = totalRecords;
                    model.CreateDate = obj.CreateDate;
                }
                if (obj.FromCustomer != null)
                {
                    model.CustomerName = obj.FromCustomer.FullName;

                }

                return model;
            }).ToList();
            return new Tuple<IList<NotificationViewModel>, int>( list, data.Item2);


        }


        public async Task<Tuple<IList<NotificationViewModel>, int>> LoadNotifcationapi( int jtStartIndex = 0, int jtPageSize = 10, string CurrentCustomerId = null)
        {
            int AllListCount = 0;
            int totalRecords = 0;
            var data = await _notificationReopsitory.LoadItemsDataApi(totalRecords, jtStartIndex, jtPageSize, CurrentCustomerId);
            AllListCount = totalRecords;


            var list = data.Item1.Select(obj =>
            {
                var model = _mapper.Map<NotificationViewModel>(obj);

                if (obj.CreateDate != null)
                {
                    model.MessageBody = obj.MessageBody;
                    model.TotalRecored = totalRecords;
                    model.CreateDate = obj.CreateDate;
                }
                if (obj.FromCustomer != null)
                {
                    model.CustomerName = obj.FromCustomer.FullName;

                }

                return model;
            }).ToList();
            return new Tuple<IList<NotificationViewModel>, int>(list, data.Item2);


        }

        public void SendNotifications(NotificationModel model, string currentCustomerId)
        {
            model.CreateDate = DateTime.Now.ReternLocalDate();
            model.IsRead = false;

            var obj = new Notification();
            _mapper.Map(model, obj);

            _notificationReopsitory.SendNotifications(obj, currentCustomerId);

            //try
            //{
            //    var userInfo = _userRepository.GetUserById(model.ToCustomerId);

            //    if (!string.IsNullOrEmpty(userInfo.MobileToken))
            //    {
            //        WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //        tRequest.Method = "post";
            //        tRequest.ContentType = "application/json";

            //        var data = new
            //        {
            //            to = userInfo.MobileToken,
            //            notification = new
            //            {
            //                title = _localizer["NotificationFrom"] + " " + userInfo.Name,
            //                body = model.MessageBody,
            //                sound = "Enabled"
            //            }
            //        };

            //        var serializer = new JavaScriptSerializer();
            //        var json = serializer.Serialize(data);
            //        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            //        tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAp0j2UFw:APA91bEh-nwMpECXeR-p3VtKikIpdfW9Tbj1j1smDSr7O93RloAGgjMJJmrb543QePY5esYdxLXew3teFzLN0C5i2c96XCzMhX4MflDRCpuI_UTnemcDTdMmkwrKG9SPQ5h-qE45Z9ti"));
            //        tRequest.Headers.Add(string.Format("Sender: id={0}", "718483640412"));
            //        tRequest.ContentLength = byteArray.Length;
            //        using (Stream dataStream = tRequest.GetRequestStream())
            //        {
            //            dataStream.Write(byteArray, 0, byteArray.Length);
            //            using (WebResponse tResponse = tRequest.GetResponse())
            //            {
            //                using (Stream dataStreamResponse = tResponse.GetResponseStream())
            //                {
            //                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
            //                    {
            //                        String sResponseFromServer = tReader.ReadToEnd();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }


    }
}
