using System;

namespace BibliaApp
{
    public static class Extensions
    {
        public static string FindInternalOf(this string data, string start, string end)
        {
            int lengthToMove = start.Length;
            int indexOfBookShortName =
                data.IndexOf(start);
            if (indexOfBookShortName < 0)
                return "";
            indexOfBookShortName += lengthToMove;
            int indexOfEndTitle = data.IndexOf(end, indexOfBookShortName);
            if (indexOfEndTitle == -1)
            {
                end = "</div><div class=\"bottom-navi\"";
                indexOfEndTitle = data.IndexOf(end, indexOfBookShortName);
            }
            if (indexOfBookShortName == indexOfEndTitle - indexOfBookShortName)
                return "";
            if (indexOfEndTitle - indexOfBookShortName <= 0)
                return "";
            return data.Substring(indexOfBookShortName, indexOfEndTitle - indexOfBookShortName);
        }

        public static string RemoveWithInternal(this string data, string start, string end)
        {
            int lengthToMove = start.Length;

            int startIndex =
                data.IndexOf(start);

            if (startIndex == -1)
                return data;

            int endIndex =
               data.IndexOf(end, startIndex + start.Length);
            if (endIndex == -1)
                return data;
            endIndex += end.Length;
            return endIndex - startIndex > 0 ? data.Remove(startIndex, endIndex - startIndex) : data;
        }

        public static Guid GenerateComb(this Guid g)
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

    }
}