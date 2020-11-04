using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinqMitXML
{
    class Program
    {
        static void Main(string[] args)
        {
            string studentsXML =
                        @"<Students>
                            <Student>
                                <Name>Toni</Name>
                                <Age>21</Age>
                                <University>Yale</University>
                                <Semester>9</Semester>
                            </Student>
                            <Student>
                                <Name>Carla</Name>
                                <Age>17</Age>
                                <University>Yale</University>
                                <Semester>8</Semester>
                            </Student>
                            <Student>
                                <Name>Leyla</Name>
                                <Age>19</Age>
                                <University>Beijing Tech</University>
                                <Semester>7</Semester>
                            </Student>
                            <Student>
                                <Name>Frank</Name>
                                <Age>25</Age>
                                <University>Beijing Tech</University>
                                <Semester>N.A.</Semester>
                            </Student>
                            <Student>
                                <Name>Frank</Name>
                                <Age>1000</Age>
                                <University>Beijing Tech</University>
                                <Semester>N.A.</Semester>
                            </Student>                            
                            <Student>
                                <Name>FehlerhafteAgeEingabe</Name>
                                <Age>öööö</Age>
                                <University>Beijing Tech</University>
                                <Semester>N.A.</Semester>
                            </Student>
                        </Students>";
            /// jede einzelne Student ist eine Zeile von unsere Tabelle,
            /// Name, Age, Uni, Semester sind die Spalten von JEDE Zeile.
            /// so ist eine alternativ unsere Tabellen in ein String zu speichern
            /// Diese TAbelle ist wie das letzte aber hat kein Gender.
            /// 
            /// video hat auch eine erklärung über generelle XML Datei...nicht so hilfreich

            // Daten auslesen... ( using System.Xml.Linq; erforderlich)
            XDocument studentsXdoc = new XDocument();
            studentsXdoc = XDocument.Parse(studentsXML);    //hiermit lesen wir den string als XML aus

            var students = from student in studentsXdoc.Descendants("Student")
                               //descendants sucht die untergeordnete Objekte in der XML mit den Name die wir übernehmen
                           select new
                           {
                               // wir wählen für jede Spalte in unsere Kollektion den Wert(Value) aus den entsprechenden Objekte(Element)
                               Name = student.Element("Name").Value,
                               Age = student.Element("Age").Value,
                               University = student.Element("University").Value,
                               Semester = student.Element("Semester").Value
                               /// HIER AUFPASSEN DASS DIE NAMEN PERFEKT GESCHRIEBEN SIND,
                               /// wenn möglich am besten via copy paste übernehmen
                           };
            /// wenn wir wieder über "var students" den maus halten, sehen wir den anonyme Typ
            /// mit Name, Age, University properties.
            /// Jetzt können wir die Properties als objekte verwenden!
            /// 

            foreach (var stu in students)
            {
                Console.WriteLine($"Der Student/in {stu.Name} ist {stu.Age} und studiert in {stu.University} und ist in Semester {stu.Semester}");
            }
            /// Herausforderung...? Semester Hinzufügen. etwas viel zu einfach...
            /// .
            /// 
            /// Herausforderung 2 sortieren nach ihrem alter (copypaste + erneut darstellen
            /// hat geklappt, hatte nur die originale Kollektio wieder ausgegeben, astatt die sortierte

            //var ageSortedStudents = from student in students orderby student.Age select student;
            // falsch sortiert mit Age als string, hier unter korrigiert. MACHT ABER EXCEPTION WENN AGE nicht geparsed sein kann TODO!!
            ////var ageSortedStudents = from student in students orderby Int32.Parse(student.Age) select student;

            ////Console.WriteLine("SORTIERT NACH ALTER");
            ////foreach (var stu in ageSortedStudents)      
            ////{
            ////    Console.WriteLine($"Der Student/in {stu.Name} ist {stu.Age} und studiert in {stu.University} und ist in Semester {stu.Semester}");
            ////}
            ///
            // erst konvertieren richtig, wenn nicht konvertierbar, dann  ageInt =0
            int ageInt = 0;
            var ageConvertedStudents = from student in students 
                                       select new
                                       {
                                           Name = student.Name,
                                           ageInt = Int32.TryParse(student.Age, out ageInt) ? ageInt : 0,       //0 als default value für ungültig.KLAPPT!
                                           University = student.University,
                                           Semester = student.Semester
                                       };

            Console.WriteLine("Jetzt richtig SORTIERT NACH ALTER");


            var ageConvertedSortedStudents = from student in ageConvertedStudents orderby student.ageInt select student;
            foreach (var stu in ageConvertedSortedStudents)
            {
                Console.WriteLine($"Der Student/in {stu.Name} ist {stu.ageInt} und studiert in {stu.University} und ist in Semester {stu.Semester}");
            }
            Console.ReadKey();
        }
    }
}
