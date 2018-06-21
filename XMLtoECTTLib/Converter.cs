using System;
using System.IO;
using System.Xml;

namespace XMLtoECTTLib
{
    public class Converter
    {
        /// <summary>
        /// Converts xml document to ectt format and writes it to file in given path 
        /// </summary>
        /// <param name="xmlDoc">XML Document to convert, must be valid against DTD schema: http://tabu.diegm.uniud.it/ctt/cb_ctt.dtd</param>
        /// <param name="pathname">Local pathname to write generated file, e.g. /path/generated_instance.ectt</param>
        public static void ConvertToECTT(XmlDocument xmlDoc, string pathname)
        {
            XmlNode instanceNode = xmlDoc.DocumentElement.SelectSingleNode("/instance");
            XmlNode descriptorNode = xmlDoc.DocumentElement.SelectSingleNode("/instance/descriptor");

            XmlNodeList coursesList = xmlDoc.DocumentElement.SelectNodes("/instance/courses/course");
            XmlNodeList roomsList = xmlDoc.DocumentElement.SelectNodes("/instance/rooms/room");
            XmlNodeList curriculaList = xmlDoc.DocumentElement.SelectNodes("/instance/curricula/curriculum");
            XmlNodeList unavailabilityContraints = xmlDoc.DocumentElement.SelectNodes("/instance/constraints/constraint/timeslot");
            XmlNodeList roomContraints = xmlDoc.DocumentElement.SelectNodes("/instance/constraints/constraint/room");

            using (StreamWriter textFile = new StreamWriter(pathname))
            {
                textFile.WriteLine("Name: " + instanceNode.Attributes["name"].Value);
                textFile.WriteLine("Courses: " + coursesList.Count);
                textFile.WriteLine("Rooms: " + roomsList.Count);
                textFile.WriteLine("Days: " + descriptorNode.SelectSingleNode("days").Attributes["value"].Value);
                textFile.WriteLine("Periods_per_day: " + descriptorNode.SelectSingleNode("periods_per_day").Attributes["value"].Value);
                textFile.WriteLine("Curricula: " + curriculaList.Count);
                textFile.WriteLine("Min_Max_Daily_Lectures: " + descriptorNode.SelectSingleNode("daily_lectures").Attributes["min"].Value + " " + descriptorNode.SelectSingleNode("daily_lectures").Attributes["max"].Value);
                textFile.WriteLine("UnavailabilityConstraints: " + unavailabilityContraints.Count);
                textFile.WriteLine("RoomConstraints: " + roomContraints.Count);

                textFile.WriteLine();
                textFile.WriteLine("COURSES:");

                foreach (XmlNode node in coursesList)
                {
                    textFile.WriteLine(node.Attributes["id"].Value + " "
                                      + node.Attributes["teacher"].Value + " "
                                      + node.Attributes["lectures"].Value + " "
                                      + node.Attributes["min_days"].Value + " "
                                      + node.Attributes["students"].Value + " "
                                      + (node.Attributes["double_lectures"].Value == "yes" ? 1 : 0));
                }


                textFile.WriteLine();
                textFile.WriteLine("ROOMS:");

                foreach (XmlNode node in roomsList)
                {
                    textFile.WriteLine(node.Attributes["id"].Value + " "
                                      + node.Attributes["size"].Value + " "
                                      + node.Attributes["building"].Value);
                }

                textFile.WriteLine();
                textFile.WriteLine("CURRICULA:");

                foreach (XmlNode node in curriculaList)
                {
                    string curriculumText = string.Empty;

                    curriculumText = node.Attributes["id"].Value + " ";

                    XmlNodeList curriculumCourses = node.SelectNodes("course");

                    curriculumText += curriculumCourses.Count;

                    foreach (XmlNode child in curriculumCourses)
                    {
                        curriculumText += " " + child.Attributes["ref"].Value;
                    }

                    textFile.WriteLine(curriculumText);
                }

                textFile.WriteLine();
                textFile.WriteLine("UNAVAILABILITY_CONSTRAINTS:");

                foreach (XmlNode node in unavailabilityContraints)
                {
                    textFile.WriteLine(node.ParentNode.Attributes["course"].Value + " " +
                                        node.Attributes["day"].Value + " " +
                                        node.Attributes["period"].Value);
                }

                textFile.WriteLine();
                textFile.WriteLine("ROOM_CONSTRAINTS:");

                foreach (XmlNode node in roomContraints)
                {
                    textFile.WriteLine(node.ParentNode.Attributes["course"].Value + " " +
                                        node.Attributes["ref"].Value);
                }


                textFile.WriteLine();
                textFile.WriteLine("END.");

            }

        }
    }
}
