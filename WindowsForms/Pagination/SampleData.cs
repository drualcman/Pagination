using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsForms.Pagination
{
    public class SampleData
    {
        public List<SampleClass> SampleList { get; set; }

        public SampleData()
        {
            SampleList = new List<SampleClass>();
            for (int i = 0; i < 1000; i++)
            {
                SampleList.Add(new SampleClass()
                {
                    Id = i,
                    Created = new DateTime(new Random().Next(1976, DateTime.Today.Year), new Random().Next(1, 12), new Random().Next(1, 28)),
                    Name = $"Name {i}",
                    Description = $"Description for name {i}",
                    Price = new Random().Next(1,1000)/100,
                    Qty = new Random().Next(1,100) 
                }); 
            }
        }

        public static List<SampleClass> GetData() 
        {
            SampleData data = new SampleData();
            return data.SampleList;

        }
    }
}
