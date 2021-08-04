using GatewaySmsAPI.DbOperations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GatewaySmsAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator.Initialize();
            var context = new DemoDBContext();

            //SentSmsToOne(context);

            //SentSmsToAll(context);

            //SentSmsToAllByUserType(context, "service");
        }

        private static void SentSmsToAllByUserType(DemoDBContext context, string userType)
        {
            var allServiceUsers = context.Users.Where<User>(x => x.UserType.ToLower() == userType.ToLower()).ToList<User>();
            if (allServiceUsers is null)
            {
                throw new InvalidOperationException("Service UserList is empty!");
            }

            var userGetDtoList = new List<UserGetDto>();
            foreach (var usr in allServiceUsers)
            {
                userGetDtoList.Add(
                    new UserGetDto()
                    {
                        UserId = usr.UserId,
                        UserType = usr.UserType,
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        OtpArea = SmsSenderHelper.GenerateOTP(),
                        PhoneNumber = usr.PhoneNumber,
                        ScheduledTime = DateTime.Now,
                        Sender = usr.Sender
                    });
            }

            foreach (var usr in userGetDtoList)
            {
                var userPartialDto = SmsSenderHelper.SendSmsMessage(usr);
                foreach (var usrr in allServiceUsers)
                {
                    usrr.MessageId = userPartialDto.MessageId;
                    usrr.Message = userPartialDto.Message;
                    usrr.Status = userPartialDto.Status;
                    usrr.SentTime = userPartialDto.SentTime;
                }
            }
        }

        private static void SentSmsToAll(DemoDBContext context)
        {
            var allUser = context.Users.ToList<User>();
            if (allUser is null)
            {
                throw new InvalidOperationException("UserList is empty!");
            }

            var userGetDtoList = new List<UserGetDto>();
            foreach (var usr in allUser)
            {
                userGetDtoList.Add(
                    new UserGetDto()
                    {
                        UserId = usr.UserId,
                        UserType = usr.UserType,
                        FirstName = usr.FirstName,
                        LastName = usr.LastName,
                        OtpArea = SmsSenderHelper.GenerateOTP(),
                        PhoneNumber = usr.PhoneNumber,
                        ScheduledTime = DateTime.Now,
                        Sender = usr.Sender
                    });
            }

            foreach (var usr in userGetDtoList)
            {
                var userPartialDto = SmsSenderHelper.SendSmsMessage(usr);
                foreach (var usrr in allUser)
                {
                    usrr.MessageId = userPartialDto.MessageId;
                    usrr.Message = userPartialDto.Message;
                    usrr.Status = userPartialDto.Status;
                    usrr.SentTime = userPartialDto.SentTime;
                }
            }
        }

        private static void SentSmsToOne(DemoDBContext context)
        {
            var user = context.Users.Where<User>(x => x.Id == 1).SingleOrDefault<User>();
            if (user is null)
            {
                throw new InvalidOperationException("User is not found!");
            }

            var userGetDto = new UserGetDto()
            {
                UserId = user.UserId,
                UserType = user.UserType,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OtpArea = SmsSenderHelper.GenerateOTP(),
                PhoneNumber = user.PhoneNumber,
                ScheduledTime = DateTime.Now,
                Sender = user.Sender
            };

            var userPartialDto = SmsSenderHelper.SendSmsMessage(userGetDto);
            user.MessageId = userPartialDto.MessageId;
            user.Message = userPartialDto.Message;
            user.Status = userPartialDto.Status;
            user.SentTime = userPartialDto.SentTime;
        }
    }
}