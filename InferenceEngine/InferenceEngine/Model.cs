using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    class Model
    {
        private List<bool> clauseTruths = new List<bool>();
        private List<List<bool>> modelList = new List<List<bool>>();

        int _size = 0;

        public Model(int size)
        {
            _size = size;
        }

        public void PopulateList(List<bool> clauseList)
        {
            List<bool> newList = new List<bool>();

            for (int i = 0; i < _size; i++)
            {
                newList = new List<bool>();

                for (int j = 0; j < clauseList.Count; j++)
                {
                    newList.Add(clauseList[j]);
                }
            }
            modelList.Add(newList);
        }

        public int IsModel()
        {
            int truths = 0;
            int models = 0;

            for (int i = 0; i < modelList[0].Count; i++) // ~2048
            {
                truths = 0;

                for (int j = 0; j < modelList.Count; j++) // ~10
                {
                    if (modelList[j][i] == true)
                    {
                        truths++;
                    }

                    if (truths == modelList.Count)
                    {
                        models++;
                    }

                    if (truths >= modelList.Count)
                    {
                        truths = 0;
                    }
                }
            }
            return models;
        }
    }
}
