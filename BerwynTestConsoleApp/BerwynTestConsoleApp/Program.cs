using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BerwynTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalRecords = 0;
            string fileName = "C:/BerwynTest/test.csv";
            string fileResultsName = "C:/BerwynTest/testResults.csv";
            bool headerRowDone = false;
            bool selectedRow = false;
            List<string> guidList = new List<string>();
            List<string> val1List = new List<string>();
            List<string> val2List = new List<string>();
            List<string> val3List = new List<string>();
            List<int> val3LengthList = new List<int>();
            List<int> valSumList = new List<int>();
            List<string> duplicateGUIDList = new List<string>();
            int valSum = 0;
            int valSumMaxIndex = 0;
            int valSumMax = 0;
            bool isDuplicateGuid = false;
            string duplicateGuid = "";
            string val3LengthGreater = "";
            int avgVal3Length = 0;

            //read file
            try
            {
                TextFieldParser tfp = new TextFieldParser(fileName);
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(",");
                while (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();

                    foreach (string field in fields)
                    {
                        if (headerRowDone)
                        {
                            if (selectedRow)
                            {
                                guidList.Add(fields[0]);
                                val1List.Add(fields[1]);
                                val2List.Add(fields[2]);
                                val3List.Add(fields[3]);
                                val3LengthList.Add(fields[3].Length);
                                selectedRow = false;
                            }
                        }
                    }
                    headerRowDone = true;
                    selectedRow = true;
                    totalRecords++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < totalRecords - 1; i++)
            {
                valSum = Convert.ToInt32(val1List[i]) + Convert.ToInt32(val2List[i]);
                valSumList.Add(valSum);
            }

            valSumMax = valSumList.Max();
            valSumMaxIndex = valSumList.IndexOf(valSumMax);

            var duplicateGUIDs = from x in guidList
                                 group x by x into grouped
                                 where grouped.Count() > 1
                                 select grouped.Key;

            foreach (string s in duplicateGUIDs)
            {
                duplicateGUIDList.Add(s);
            }

            avgVal3Length = (val3LengthList.Sum() / totalRecords);

            Console.SetWindowSize(100, 50);
            Console.WriteLine("Total Number of Records in File: " + (totalRecords - 1));
            Console.WriteLine("Largest Max value for Val1 + Val2, Guid:");
            Console.WriteLine(valSumMax.ToString() + ", " + guidList[valSumMaxIndex]);
            Console.WriteLine("Duplicate GUIDs in File: ");
            for (int i = 0; i < duplicateGUIDList.Count; i++)
            {
                Console.WriteLine(duplicateGUIDList[i]);
            }
            Console.WriteLine("Average Length of Val3 in File: " + avgVal3Length.ToString());

            var sb = new StringBuilder();
            //write header
            string newLine = string.Format("{0},{1},{2},{3}", "GUID", "Val1+Val2", "IsDuplicateGuid (Y or N)", "Is Val3 length greater than the average length of Val3 (Y or N)");
            sb.AppendLine(newLine);

            //write file
            try
            {

                for (int i = 0; i < totalRecords - 1; i++)
                {
                    isDuplicateGuid = duplicateGUIDs.Contains(guidList[i]);

                    if (isDuplicateGuid)
                    {
                        duplicateGuid = "Y";
                    }
                    else
                    {
                        duplicateGuid = "N";
                    }

                    if (val3LengthList[i] > avgVal3Length)
                    {
                        val3LengthGreater = "Y";
                    }
                    else
                    {
                        val3LengthGreater = "N";
                    }

                    newLine = string.Format("{0},{1},{2},{3}", guidList[i], Convert.ToInt32(val1List[i]) + Convert.ToInt32(val2List[i]), duplicateGuid, val3LengthGreater);
                    sb.AppendLine(newLine);
                }

                File.WriteAllText(fileResultsName, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //pause window so user can view results
            Console.ReadKey();
        }
    }
}
