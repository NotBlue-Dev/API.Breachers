using System.Collections.Generic;

namespace BreachersAPI.ObjectData {
    internal class ObjManager {
        private int UpdateCount = 0;
        private int everyXUpdate = 1000;
        public void GetAllObjects() {
            if(UpdateCount > everyXUpdate) {
                List<BaseObj> classList = new List<BaseObj> { new NetGameDataObj() };

                foreach (BaseObj item in classList) {
                    item.GetObjects();
                }
                
                UpdateCount = 0;
            }
            
            UpdateCount++;
        }
    }
}
