﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace temp3
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream f = new FileStream(@"C:\Users\Training\Downloads\CSV\Indicators.csv", FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(f);
            FileStream fs = new FileStream(@"C:\Users\Training\Downloads\CSV\life-xpctncy1.json", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter write = new StreamWriter(fs);
            string[] country = { "AFG", "ARM", "AZE", "BHR", "BGD", "BTN", "BRN", "KHM", "CHN", "CXR", "CCK", "IOT", "GEO", "HKG", "IND", "IDN", "IRN",
                "IRQ", "ISR", "JPN", "JOR", "KAZ", "KWT", "KGZ", "LAO", "LBN", "MAC", "MYS", "MDV", "MNG", "MMR", "NPL", "PRK", "OMN", "PAK",
                "PHL", "QAT", "SAU", "SGP", "KOR", "LKA", "SYR", "TWN", "TJK", "THA", "TUR", "TKM", "ARE", "UZB", "VNM", "YEM", "SAS", "EAS" };  // Array of ASIAN Countries
            var str = read.ReadLine();
            string[] words = str.Split(',');          
            string space;
            float valueForAgg = 0, valueForFemaleAgg = 0;        
            Dictionary<string, float> Agg_Male = new Dictionary<string, float>();
            Dictionary<string, float> Agg_Female = new Dictionary<string, float>();
            while ((space = read.ReadLine()) != null)
            {
                string[] val = space.Split(',');     
                for (int i = 0; i < val.Length; i++)
                {
                    if (val[i].StartsWith("\""))
                    {
                        if (val[i].EndsWith("\"")) { }
                        else
                        {
                            val[i] = val[i] + val[i + 1];
                            val = val.Where((value, idx) => idx != (i + 1)).ToArray();
                        }
                    }       //end of if
                }       //end of for
                foreach (var i in country)
                {
                    if (val[1] == i)
                    {
                        if (val[2] == "\"Life expectancy at birth male (years)\"")
                        {
                            valueForAgg += float.Parse(val[5]);

                            if (!Agg_Male.ContainsKey(val[1]))
                            {
                                Agg_Male.Add(val[1], float.Parse(val[5]));
                            }
                            else
                            {
                                Agg_Male[val[1]] += float.Parse(val[5]);
                            }

                        }       //end of nested if
                        else if (val[2] == "\"Life expectancy at birth female (years)\"")
                        {
                            valueForFemaleAgg += float.Parse(val[5]);
                            if (!Agg_Female.ContainsKey(val[1]))
                            {
                                Agg_Female.Add(val[1], float.Parse(val[5]));
                            }
                            else
                            {
                                Agg_Female[val[1]] += float.Parse(val[5]);
                            }
                        }       //end of else if
                    }       //end of if
                }       //end of foreach
            }       //end of while
            int count = 0;
            write.WriteLine("[");
            foreach (KeyValuePair<string, float> entry in Agg_Male)
            {
                write.WriteLine("{");
                write.WriteLine("\"CountryCode\":" + "\""+ entry.Key + "\"" + ",");
                write.WriteLine("\"Life_expectancy_at_birth_male\":" + entry.Value + ",");
                write.WriteLine("\"Life_expectancy_at_birth_female\":" + Agg_Female[entry.Key]);
                count++;
                write.WriteLine("}");
                if (count != Agg_Male.Count)
                {
                    write.WriteLine(",");
                }
            }       //end of for each
            write.WriteLine("]");
            write.Flush();
        }       
    }
}