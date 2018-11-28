using System;

namespace StreamingDatasetsDemo {
  class Program {

    static PowerBiServiceWrapper pbiService = new PowerBiServiceWrapper();

    static void Main() {
      
      pbiService.CreateDemoStreamingDataset("Demo 1: Streaming Dataset");
      // pbiService.CreateDemoHybridDataset("Demo 2: Hybrid Dataset");
      // pbiService.CreateDemoPushDataset("Demo 3: Push Dataset");
      
    }
  }

}
