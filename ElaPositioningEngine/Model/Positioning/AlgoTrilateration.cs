using ElaPositioningEngine.Model.RawData;
using System;
using System.Collections.Generic;

namespace ElaPositioningEngine.Model.Positioning
{
    public class AlgoTrilateration
    {
        public double Xt;
        public double Yt;
        public List<ertlsRawDataItem> sortedRawDataList;

        public string getKey(int index1, int index2, int index3)
        {
            List<int> sorted = new List<int>();
            sorted.Add(index1);
            sorted.Add(index2);
            sorted.Add(index3);
            sorted.Sort(); 

            return string.Format("{0}:{1}:{2}", sorted[0], sorted[1], sorted[2]);
        }

        private Dictionary<string, Tuple<int, int, int, int>> sortDataToProcess(List<ertlsRawDataItem> sortedRawDataList)
        {
            Dictionary<string, Tuple<int, int, int, int>> tempDictionnarySorting = new Dictionary<string, Tuple<int, int, int, int>>();

            int iCounter = 0;
            int iMaxIndexToProcess = Math.Min(4, sortedRawDataList.Count);
            for (int i = 0; i < iMaxIndexToProcess; i++)
            {
                for (int j = (i + 1); j < iMaxIndexToProcess; j++)
                {
                    for (int k = (j + 1); k < iMaxIndexToProcess; k++)
                    {
                        string key = getKey(i, j, k);
                        if (false == tempDictionnarySorting.ContainsKey(key))
                        {
                            tempDictionnarySorting.Add(key, new Tuple<int, int, int, int>(iCounter, i, j, k));
                            iCounter++;
                        }
                    }
                }
            }

            return tempDictionnarySorting;
        }

        public AlgoTrilateration(object m_loc_payload)
        {
            Xt = 0;
            Yt = 0;

            ertlsRawData eRawData = new ertlsRawData(m_loc_payload);
            sortedRawDataList = eRawData.getListSortRssi();


            if (sortedRawDataList.Count >= 3)
            {
                // then group data into 
                foreach (KeyValuePair<string, Tuple<int, int, int, int>> indexes in sortDataToProcess(sortedRawDataList))
                {
                    /** Algorithm:
                     * delta = (x2-x1)*(y3-y1)] - [(x3-x1)*(y2-y1)
                     * B1 = d1^2 - d2^2 - x1^2 + x2^2 - y1^2 + y2^2
                     * B2 = d1^2 - d3^2 - x1^2 + x3^2 - y1^2 + y3^2
                     * Xt = [B1*(y3-y1) + B2*(y1-y2)]/(2*delta)
                     * Yt = [B1*(x1-x3) + B2*(x2-x1)]/(2*delta)
                     */

                    // trilateration for single group
                    double delta = (sortedRawDataList[indexes.Value.Item3].x_coordinate - sortedRawDataList[indexes.Value.Item2].x_coordinate) * (sortedRawDataList[indexes.Value.Item4].y_coordinate - sortedRawDataList[indexes.Value.Item2].y_coordinate) - (sortedRawDataList[indexes.Value.Item4].x_coordinate - sortedRawDataList[indexes.Value.Item2].x_coordinate) * (sortedRawDataList[indexes.Value.Item3].y_coordinate - sortedRawDataList[indexes.Value.Item2].y_coordinate);
                    double B1 = square(sortedRawDataList[indexes.Value.Item2].eDistance) - square(sortedRawDataList[indexes.Value.Item3].eDistance) - square(sortedRawDataList[indexes.Value.Item2].x_coordinate) + square(sortedRawDataList[indexes.Value.Item3].x_coordinate) - square(sortedRawDataList[indexes.Value.Item2].y_coordinate) + square(sortedRawDataList[indexes.Value.Item3].y_coordinate);
                    double B2 = square(sortedRawDataList[indexes.Value.Item2].eDistance) - square(sortedRawDataList[indexes.Value.Item4].eDistance) - square(sortedRawDataList[indexes.Value.Item2].x_coordinate) + square(sortedRawDataList[indexes.Value.Item4].x_coordinate) - square(sortedRawDataList[indexes.Value.Item2].y_coordinate) + square(sortedRawDataList[indexes.Value.Item4].y_coordinate);
                    Xt += (B1 * (sortedRawDataList[indexes.Value.Item4].y_coordinate - sortedRawDataList[indexes.Value.Item2].y_coordinate) + B2 * (sortedRawDataList[indexes.Value.Item2].y_coordinate - sortedRawDataList[indexes.Value.Item3].y_coordinate)) / (2 * delta);
                    Yt += (B1 * (sortedRawDataList[indexes.Value.Item2].x_coordinate - sortedRawDataList[indexes.Value.Item4].x_coordinate) + B2 * (sortedRawDataList[indexes.Value.Item3].x_coordinate - sortedRawDataList[indexes.Value.Item2].x_coordinate)) / (2 * delta);

                }
                if (sortedRawDataList.Count > 3)
                {
                    Xt /= sortedRawDataList.Count;
                    Yt /= sortedRawDataList.Count;
                }
            }
            else if (sortedRawDataList.Count == 2)
            {
                Xt = (sortedRawDataList[0].x_coordinate / sortedRawDataList[0].eDistance + sortedRawDataList[1].x_coordinate / sortedRawDataList[1].eDistance) / (1 / sortedRawDataList[0].eDistance + 1 / sortedRawDataList[1].eDistance);
                Yt = (sortedRawDataList[0].y_coordinate / sortedRawDataList[0].eDistance + sortedRawDataList[1].y_coordinate / sortedRawDataList[1].eDistance) / (1 / sortedRawDataList[0].eDistance + 1 / sortedRawDataList[1].eDistance);
            }
            else if (sortedRawDataList.Count == 1)
            {
                Xt = sortedRawDataList[0].x_coordinate;
                Yt = sortedRawDataList[0].y_coordinate;
            }
        }

        private static double square(double item)
        {
            return Math.Pow(item, 2);
        }
    }
}
