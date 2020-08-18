using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.Extension
{
    public static class CachProvider
    {
        const string OnlineMemberKey = "OnlineMember";
        const string OnlineUserKey = "OnlineUsers";
        

        public static int getTodayRegisterMembers(this IDistributedCache cache)
        {
            var CurrentDay = cache.GetString("TodayRegisterMembers");
            if (string.IsNullOrEmpty(CurrentDay))
                return 0;
            else
                return int.Parse(CurrentDay);
        }
        public static void setTodayRegisterMembers(this IDistributedCache cache, int day)
        {
            cache.SetString("TodayRegisterMembers", day.ToString());
        }







        public static int getTodayVisitMembers(this IDistributedCache cache)
        {
            var CurrentDay = cache.GetString("TodayVisitMembers");
            if (string.IsNullOrEmpty(CurrentDay))
                return 0;
            else
                return int.Parse(CurrentDay);
        }

        public static void setTodayVisitMembers(this IDistributedCache cache, int day)
        {
            cache.SetString("TodayVisitMembers", day.ToString());
        }



        public static int getCurrentDay(this IDistributedCache cache)
        {
            var CurrentDay = cache.GetString("CurrentDay");
            if (string.IsNullOrEmpty(CurrentDay))
                return 0;
            else
                return int.Parse(CurrentDay);
        }

        public static void setCurrentDay(this IDistributedCache cache,int day)
        {
            cache.SetString("CurrentDay", day.ToString());
        }




        public static int countOnlineMembers(this IDistributedCache cache)
        {
            var members = cache.Get(OnlineMemberKey);
            if (members != null)
                return members.FromByteArray<List<(int, DateTime)>>().Count();
            else
                return 0;
        }

        public static List<(int, DateTime)> getOnlineMembers(this IDistributedCache cache)
        {
            var members = cache.Get(OnlineMemberKey);
            if (members != null)
                return members.FromByteArray<List<(int, DateTime)>>();
            else
                return new List<(int, DateTime)>();
        }

        public static void setOnlineMembers(this IDistributedCache cache, List<(int, DateTime)> members)
        {
            cache.Set(OnlineMemberKey, members.ToByteArray());
        }

        /// <summary>
        /// Add or Update Member in online Members
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="member"></param>
        public static void addOnlineMembers(this IDistributedCache cache, (int, DateTime) member)
        {
            var membersByte = cache.Get(OnlineMemberKey);
            if (membersByte != null)
            {
                var members = membersByte.FromByteArray<List<(int, DateTime)>>();
                if (members.Any(a => a.Item1 == member.Item1))
                    members[members.FindIndex(f => f.Item1 == member.Item1)] = member;
                else
                    members.Add(member);
                
                cache.Set(OnlineMemberKey, members.ToByteArray());
            }
            else
            {
                var members = new List<(int, DateTime)>();
                members.Add(member);
                cache.Set(OnlineMemberKey, members.ToByteArray());
            }
        }







        public static int countOnlineUsers(this IDistributedCache cache)
        {
            var users = cache.Get(OnlineUserKey);
            if (users != null)
                return users.FromByteArray<List<(int, DateTime)>>().Count();
            else
                return 0;
        }

        public static List<(int, DateTime)> getOnlineUsers(this IDistributedCache cache)
        {
            var users = cache.Get(OnlineUserKey);
            if (users != null)
                return users.FromByteArray<List<(int, DateTime)>>();
            else
                return new List<(int, DateTime)>();
        }

        public static void setOnlineUsers(this IDistributedCache cache, List<(int, DateTime)> users)
        {
            cache.Set(OnlineUserKey, users.ToByteArray());
        }

        /// <summary>
        /// Add or Update Member in online Members
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="member"></param>
        public static void addOnlineUsers(this IDistributedCache cache, (int, DateTime) user)
        {
            var usersByte = cache.Get(OnlineUserKey);
            if (usersByte != null)
            {
                var users = usersByte.FromByteArray<List<(int, DateTime)>>();
                if (users.Any(a => a.Item1 == user.Item1))
                    users[users.FindIndex(f => f.Item1 == user.Item1)] = user;
                else
                    users.Add(user);
                cache.Set(OnlineUserKey, users.ToByteArray());
            }
            else
            {
                var users = new List<(int, DateTime)>();
                users.Add(user);
                cache.Set(OnlineUserKey, users.ToByteArray());
            }
        }
    }


    public static class Serialization
    {
        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return default(T);
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }

    }
}
