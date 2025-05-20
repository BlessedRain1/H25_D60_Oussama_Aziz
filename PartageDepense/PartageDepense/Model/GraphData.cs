using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class GraphData
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }

        public GraphData(string label, double value, DateTime date)
        {
            Label = label;
            Value = value;
            Date = date;
        }

        // Constructeur pour la rétrocompatibilité
        public GraphData(string label, double value) : this(label, value, DateTime.Now)
        {
        }
    }
}
