using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingDatasetsDemo.Models {

  public class TemperatureReadingsRow {
    public string Run { get; set; }
    public DateTime Time { get; set; }
    public string TimeWindow { get; set; }
    public double TargetTemperature { get; set; }
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public double BatchA { get; set; }
    public double BatchB { get; set; }
    public double BatchC { get; set; }
  }

  class TemperatureReadingsRows {
    public TemperatureReadingsRow[] rows { get; set; }
  }


}
