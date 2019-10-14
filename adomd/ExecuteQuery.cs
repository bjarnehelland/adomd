using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace adomd
{
    public class ExecuteQuery
    {
        private readonly string _connection;
        private readonly string _query;

        public ExecuteQuery(string connection, string query)
        {
            _connection = connection;
            _query = query;
        }

        public List<ExpandoObject> Run() 
        {
            AdomdConnection conn = new AdomdConnection(_connection);
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
            AdomdCommand cmd = new AdomdCommand(_query, conn);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities


            conn.Open();
            var result = cmd.ExecuteCellSet();
            conn.Close();


            var rows = new List<ExpandoObject>();
            if (result.Axes.Count == 2)
            {
                foreach (var item in result.Axes[1].Positions)
                {
                    var row = new ExpandoObject() as IDictionary<string, Object>;

                    foreach (var member in item.Members)
                    {
                        var propName = ExtractPropName(member);
                        row.Add(propName + "Id", ExtractId(member));
                        row.Add(propName + "Caption", member.Caption);

                    }
                    foreach (var position in result.Axes[0].Positions)
                    {
                        var propName = MakePropNameUnique(row, ExtractPropName(position.Members));
                        row.Add(propName, result.Cells[position.Ordinal, item.Ordinal].Value);
                    }

                    rows.Add(row as ExpandoObject);
                }
            }

            return rows;
        }

        private string ExtractPropName(Member member)
        {
            var propName = member.UniqueName;

            var index = propName.IndexOf(".&");
            if (index != -1)
            {
                propName = propName.Substring(0, index);
            }



            return propName.Split('.').Last().Replace("[", "").Replace("]", "");
        }

        private string ExtractId(Member member)
        {
            var propName = member.UniqueName;

            var index = propName.IndexOf(".&");
            if (index != -1)
            {
                propName = propName.Substring(index + 2).Replace("[", "").Replace("]", "");
            }

            return propName.Split('.').Last().Replace("[", "").Replace("]", "");
        }

        private string ExtractPropName(MemberCollection members)
        {
            var propName = "";

            foreach (var member in members)
            {
                propName += ExtractPropName(member);
            }

            return propName;
        }

        private string MakePropNameUnique(IDictionary<string, object> row, string propName)
        {
            if (!row.ContainsKey(propName))
            {
                return propName;
            }

            int count = 1;

            while (row.ContainsKey(propName + count))
            {
                count++;
            }
            return propName + count;

        }
    }
}
