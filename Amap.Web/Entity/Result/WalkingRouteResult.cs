using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Devices.Geolocation;

namespace Amap.Web.Entity.Result
{
    public class PathsItem
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public string distance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string duration { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<StepsItem> steps { get; set; }

        #endregion 属性
    }

    public class Route
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        public string destination { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string origin { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<PathsItem> paths { get; set; }

        #endregion 属性
    }

    public class StepsItem
    {
        #region 属性

        /// <summary>
        /// 右转
        /// </summary>
        public string action { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string assistant_action { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string distance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string duration { get; set; }

        /// <summary>
        /// 向西步行196米右转
        /// </summary>
        public string instruction { get; set; }

        /// <summary>
        /// 西
        /// </summary>
        public string orientation { get; set; }

        /// <summary>
        /// 路径字符串
        /// </summary>
        [JsonProperty("polyline")]
        public string PolyLine
        {
            get { return _polyLine; }
            set
            {
                _polyLine = value;
                try
                {
                    if (value != null)
                    {
                        PolyLineList = new List<BasicGeoposition>();
                        var polylines = value.Split(';');
                        foreach (var polyPoint in polylines)
                        {
                            var points = polyPoint.Split(',');
                            if (points.Length == 2)
                            {
                                float.TryParse(points[0], out var lgt);
                                float.TryParse(points[1], out var lat);

                                PolyLineList.Add(new BasicGeoposition() { Latitude = lat, Longitude = lgt });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// 路径的BasicGeoposition集合
        /// </summary>
        public List<BasicGeoposition> PolyLineList { get; set; }

        /// <summary>
        /// 四路
        /// </summary>
        public string road { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string walk_type { get; set; }

        /// <summary>
        ///
        /// </summary>
        private string _polyLine { get; set; }

        #endregion 属性
    }

    /// <summary>
    /// 行走路线请求结果
    /// </summary>
    public class WalkingRouteResult : BaseResult
    {
        #region 属性

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Route route { get; set; }

        #endregion 属性
    }
}