using Accord.MachineLearning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Point
{
    public enum PointType
    {
        Move,
        Stop,
        CheckIn,
        CheckOut,
        Offline,
    };

    public double Lat { get; set; }
    public double Lng { get; set; }
    public int OrigIndex { get; set; }
    public DateTime Time { get; set; }
    public PointType Type { get; set; }
    public DateTime thoigianbantin { get; set; }
    public string tennhanvien { get; set; }
    public string tinhtrangpin { get; set; }

    public int idnhanvien { get; set; }
    public double accuracy { get; set; }
    public string tenkhachhang { get; set; }
    public string diachikhachhang { get; set; }
    public int idkhachhang { get; set; }
    public DateTime thoigiantaidiem { get; set; }
    public DateTime thoigianvaodiem { get; set; }
    public DateTime thoigianradiem { get; set; }
    public DateTime thoigianketthuc { get; set; }//thời gian kết thúc của điểm dừng
	public double speed { get; set; } // truong up thu luc 10:13
    public double max_accuracy { get; set; } //accuray max của điểm dừng
    public double ThoiGianDungDo_Giay
    {
        get
        {
            double tg = 0;
            if(speed == 0 && thoigianketthuc.Year > 2000)
            {
                TimeSpan tsDung = thoigianketthuc - thoigianbantin;
                tg = tsDung.TotalSeconds > 20 ? tsDung.TotalSeconds : 0;
            }
           
            return tg;
        }

        
    }

     
   
}

public class LoTrinhGPSFilter
{
    private double _tongKM;


    log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoTrinhGPSFilter));
    List<Point> OrigPoints;

    public double tongKM
    {
        get
        {
            return Math.Round(_tongKM, 2);
        }

        set
        {
            _tongKM = value;
        }
    }

    public LoTrinhGPSFilter()
    {

    }
    public LoTrinhGPSFilter(List<Point> inputs)
    {
        this.OrigPoints = new List<Point>(inputs);
    }

    //public List<Point> Filter(int inStopSegmentDistanceThreshold, int minEnterStopSegment, int minEscapeStopSegment, int kFilter, int dFilter)
    //{
    //    List<Point> filteredPoints = new List<Point>(OrigPoints);

    //    try
    //    {
    //        // STEP 1: Get Stop-segments
    //        List<Dictionary<String, int>> StopSegments = new List<Dictionary<String, int>>();

    //        int countEnterSegment = 0;
    //        int countEscapeSegment = 0;
    //        int prevPointIndex = -1;
    //        int currPointIndex = -1;

    //        bool isInsideStopSegment = false;
    //        List<Point> currStopSegmentPoints = new List<Point>();
    //        int currStopSegmentFirstIndex = -1;
    //        int currStopSegmentLastIndex = -1;
    //        Point prevPoint = null;
    //        Point currPoint = null;

    //        while (currPointIndex < OrigPoints.Count - 1)
    //        {
    //            currPointIndex++;

    //            if (prevPointIndex == -1)
    //            {
    //                // first point 
    //                prevPointIndex = 0;
    //                continue;
    //            }

    //            if (OrigPoints[prevPointIndex].Type == Point.PointType.CheckIn
    //                || OrigPoints[prevPointIndex].Type == Point.PointType.CheckOut)
    //            {
    //                // if prev point is a checkin/checkout point => skip checking
    //                prevPointIndex = currPointIndex;
    //                continue;
    //            }

    //            currPoint = OrigPoints[currPointIndex];
    //            prevPoint = OrigPoints[prevPointIndex];

    //            // check for CheckIn/CheckOut point
    //            if (OrigPoints[currPointIndex].Type == Point.PointType.CheckIn
    //                || OrigPoints[currPointIndex].Type == Point.PointType.CheckOut)
    //            {
    //                // escape stop-segment if currently inside one
    //                if (isInsideStopSegment)
    //                {
    //                    StopSegments.Add(new Dictionary<String, int> { { "start", currStopSegmentFirstIndex }, { "stop", currStopSegmentLastIndex } });
    //                    isInsideStopSegment = false;
    //                    countEnterSegment = 0;
    //                    countEscapeSegment = 0;
    //                    currStopSegmentPoints.Clear();
    //                    currStopSegmentFirstIndex = -1;
    //                    currStopSegmentLastIndex = -1;
    //                }
    //                prevPointIndex = currPointIndex;
    //                continue;
    //            }

    //            // get distance from prev point or current stopsegment center point (if inside one) 
    //            double distance = (currStopSegmentFirstIndex == -1) ? GetDistance(currPoint, prevPoint) : GetDistance(currPoint, GetMeanPoint(currStopSegmentPoints));

    //            if (distance <= inStopSegmentDistanceThreshold)
    //            {
    //                if (!isInsideStopSegment)
    //                {
    //                    // not inside a stop-segment
    //                    if (countEnterSegment == 0)
    //                    {
    //                        // second point of an about-to-enter stop-segment -> set prev point to be the first index of the stop segment
    //                        currStopSegmentFirstIndex = prevPointIndex;

    //                        // add prev point to currStopSegmentPoint list
    //                        currStopSegmentPoints.Add(prevPoint);
    //                    }

    //                    countEnterSegment++;

    //                    // add current point to currStopSegmentPoint list
    //                    currStopSegmentPoints.Add(currPoint);

    //                    if (countEnterSegment < minEnterStopSegment)
    //                    {
    //                        // still in about-to-enter zone
    //                        prevPointIndex = currPointIndex;
    //                        continue;
    //                    }
    //                    else
    //                    {
    //                        // officially enter a stop-segment
    //                        isInsideStopSegment = true;
    //                        currStopSegmentLastIndex = currPointIndex;
    //                        prevPointIndex = currPointIndex;
    //                        continue;
    //                    }
    //                }
    //                else
    //                {
    //                    // currently inside a stop-segment

    //                    if (countEscapeSegment > 0)
    //                    {
    //                        // opt-out of about-to-escape zone
    //                        countEscapeSegment = 0;
    //                        currStopSegmentLastIndex = -1;
    //                    }
    //                    else
    //                    {
    //                        currStopSegmentPoints.Add(currPoint);
    //                    }

    //                    currStopSegmentLastIndex = currPointIndex;

    //                    if (currPointIndex == OrigPoints.Count - 1)
    //                        // last point of path
    //                        StopSegments.Add(new Dictionary<String, int> { { "start", currStopSegmentFirstIndex }, { "stop", currStopSegmentLastIndex } });

    //                    prevPointIndex = currPointIndex;
    //                    continue;
    //                }
    //            }
    //            else
    //            {
    //                // distance > inStopSegmentDistanceThreshold
    //                if (!isInsideStopSegment)
    //                {
    //                    // not inside a stop segment
    //                    // opt-out of about-to-enter zone
    //                    countEnterSegment = 0;
    //                    currStopSegmentPoints.Clear();
    //                    currStopSegmentFirstIndex = -1;
    //                    currStopSegmentLastIndex = -1;
    //                }
    //                else
    //                {
    //                    // inside a stop segment

    //                    if (countEscapeSegment == 0)
    //                    {
    //                        // enter about-to-escape-stop-segment zone
    //                        currStopSegmentLastIndex = prevPointIndex;
    //                    }

    //                    if (currPointIndex == OrigPoints.Count - 1)
    //                        // last point of path
    //                        StopSegments.Add(new Dictionary<String, int> { { "start", currStopSegmentFirstIndex }, { "stop", currStopSegmentLastIndex } });


    //                    countEscapeSegment++;

    //                    if (countEscapeSegment > minEscapeStopSegment)
    //                    {
    //                        // officially escape stop segment
    //                        StopSegments.Add(new Dictionary<String, int> { { "start", currStopSegmentFirstIndex }, { "stop", currStopSegmentLastIndex } });
    //                        isInsideStopSegment = false;
    //                        countEnterSegment = 0;
    //                        countEscapeSegment = 0;
    //                        currStopSegmentPoints.Clear();

    //                        currPointIndex = currStopSegmentLastIndex + 1;
    //                        prevPointIndex = currStopSegmentLastIndex + 1;

    //                        currStopSegmentFirstIndex = -1;
    //                        currStopSegmentLastIndex = -1;
    //                    }
    //                }

    //                prevPointIndex = currPointIndex;
    //                continue;
    //            }
    //        }

    //        Debug.Print("End getting stop segments");

    //        // STEP 2: REMOVE SEGMENT OUTLIERS, GET SEGMENT CENTER POINT, REPLACE IN PATH
    //        int segmentIndex = 0;
    //        for (segmentIndex = StopSegments.Count - 1; segmentIndex >= 0; segmentIndex--)
    //        {
    //            Dictionary<String, int> segment = StopSegments[segmentIndex];

    //            int segmentStartIndex = segment["start"];
    //            int segmentEndIndex = segment["stop"];
    //            int segmentSize = segmentEndIndex - segmentStartIndex + 1;

    //            // get segment points
    //            List<Point> segmentPoints = new List<Point>();
    //            int pointIndex = 0;
    //            int[] kNNOutputs = new int[segmentSize];
    //            for (pointIndex = segmentStartIndex; pointIndex <= segmentEndIndex; pointIndex++)
    //            {
    //                segmentPoints.Add(OrigPoints[pointIndex]);
    //                kNNOutputs[pointIndex - segmentStartIndex] = 0;
    //            }

    //            // init k-nearest neighbors
    //            int k = (Math.Ceiling((double)(kFilter * segmentSize / 100)) > 1) ? (int)Math.Ceiling((double)(kFilter * segmentSize) / 100) : 1;
    //            Func<Point, Point, double> kNNDistanceFunc = GetDistance;
    //            Point[] kNNInputs = segmentPoints.ToArray();

    //            KNearestNeighbors<Point> kNN = new KNearestNeighbors<Point>(k + 1, kNNInputs, kNNOutputs, kNNDistanceFunc); // k+1: including the point itself

    //            // remove outliers
    //            List<int> outliers = new List<int>();
    //            int pIndex = 0;
    //            for (pIndex = 0; pIndex < segmentPoints.Count; pIndex++)
    //            {
    //                Point point = segmentPoints[pIndex];

    //                // get (k+1)-nearest neighbor (including the point itself)
    //                int[] labels = new int[k + 1];
    //                Point[] neighbors = kNN.GetNearestNeighbors(point, out labels);
    //                foreach (Point neighbor in neighbors)
    //                {
    //                    if (GetDistance(neighbor, point) > dFilter)
    //                    {
    //                        outliers.Add(pIndex);
    //                        break;
    //                    }
    //                }
    //            }
    //            outliers.Sort();
    //            outliers.Reverse();
    //            Debug.Print("Removing " + outliers.Count + " outliers from segment");
    //            foreach (int outlierIndex in outliers)
    //                segmentPoints.RemoveAt(outlierIndex);

    //            // get segment center point
    //            Point meanPoint = GetMeanPoint(segmentPoints);
    //            meanPoint.Type = Point.PointType.Stop;
    //            meanPoint.OrigIndex = OrigPoints[segmentStartIndex].OrigIndex;
    //            Debug.Print("Segment center point: " + meanPoint.Lat + " - " + meanPoint.Lng);

    //            // replace segment with center point
    //            Debug.Print("Replacing " + segmentSize + " points");
    //            filteredPoints.RemoveRange(segmentStartIndex, segmentSize);
    //            filteredPoints.Insert(segmentStartIndex, meanPoint);
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    return filteredPoints;
    //}

    public List<Point> FilterKhoangCachTheoThoiGian(List<Point> pointCu)
    {
        List<Point> pointMoi = new List<Point>();
        int viTriCu = 1;
        for (int i = 1; i < pointCu.Count; i++)
        {
            double met = GetDistance(pointCu[viTriCu], pointCu[i]);
            double time = (pointCu[i].thoigianbantin - pointCu[viTriCu].thoigianbantin).TotalSeconds;
            double metTrenGiay = met / time;
            //80km/h = 22m/s
            if (metTrenGiay <= 22)
            {
                pointMoi.Add(pointCu[i]);
                viTriCu = i;
            }
        }
        return pointMoi;
    }

    public double GetDistance(Point p1, Point p2)
    {
        return GeoCodeCalc.CalcDistance(p1.Lat, p1.Lng, p2.Lat, p2.Lng, GeoCodeCalcMeasurement.Kilometers) * 1000;
    }


    public double GetDistance_WithAccuracy(Point p1, Point p2)
    {
        return GeoCodeCalc.CalcDistance(p1.Lat, p1.Lng, p2.Lat, p2.Lng, GeoCodeCalcMeasurement.Kilometers) * 1000  - (p1.accuracy + p2.accuracy);

    }
    private Point GetMeanPoint(List<Point> points)
    {
        double totalLat = 0;
        double totalLng = 0;
        foreach (Point p in points)
        {
            totalLat += p.Lat;
            totalLng += p.Lng;
        }
        double meanLat = totalLat / points.Count;
        double meanLng = totalLng / points.Count;

        Point retPoint = new Point();
        retPoint.Lat = Math.Round(meanLat, 7);
        retPoint.Lng = Math.Round(meanLng, 7);

        return retPoint;
    }

    //Hàm filter Trường NM sửa ngày 17/10 : Filter theo accuracy

    public List<Point> Filter()
    {
        List<Point> filteredPoints = new List<Point>();
        List<Point> pointDungDo = new List<Point>();

        for (int i = 0; i < OrigPoints.Count; i++)
        {
            if (i == 0)
            {
                filteredPoints.Add(OrigPoints[i]);
            }
            else
            {
                double kc = GetDistance(OrigPoints[i - 1], OrigPoints[i]);
                //kiem tra neu khoang cach giua 2 diem > ban kinh sai lech => then vao danh sach cac diem se phai loai bo
                if (OrigPoints[i].accuracy > kc)
                {
                    if (OrigPoints[i].accuracy > OrigPoints[i - 1].accuracy)
                    {
                        OrigPoints[i].Lat = OrigPoints[i - 1].Lat;
                        OrigPoints[i].Lng = OrigPoints[i - 1].Lng;

                    }
                    else
                    {
                        OrigPoints[i - 1].Lat = OrigPoints[i].Lat;
                        OrigPoints[i - 1].Lng = OrigPoints[i].Lng;
                    }
                    pointDungDo.Add(OrigPoints[i]);
                }
                else
                {
                    if (pointDungDo.Count > 1)
                    {
                        filteredPoints.Add(pointDungDo[0]);
                        pointDungDo[pointDungDo.Count - 1].Lat = pointDungDo[0].Lat;
                        pointDungDo[pointDungDo.Count - 1].Lng = pointDungDo[0].Lng;
                        filteredPoints.Add(pointDungDo[pointDungDo.Count - 1]);
                    }
                    else if (pointDungDo.Count == 1)
                    {
                        filteredPoints.Add(pointDungDo[0]);
                    }
                    pointDungDo = new List<Point>();
                    if (OrigPoints[i].accuracy > OrigPoints[i - 1].accuracy)
                    {
                        OrigPoints[i].Lat = OrigPoints[i - 1].Lat;
                        OrigPoints[i].Lng = OrigPoints[i - 1].Lng;
                    }
                    else
                    {
                        OrigPoints[i - 1].Lat = OrigPoints[i].Lat;
                        OrigPoints[i - 1].Lng = OrigPoints[i].Lng;
                    }

                    filteredPoints.Add(OrigPoints[i]);
                }
            }
        }

        return filteredPoints;
    }
    public List<Point> FilterTheoAccuracy(List<Point> pointCu)
    {
        List<Point> pointMoi = new List<Point>();
        int viTriPointSoSanh = 0;
        if (pointCu.Count == 0)
        {
            return pointMoi;
        }
        pointMoi.Add(pointCu[0]);
        for (int i = 1; i < pointCu.Count; i++)
        {
            bool namTrong = KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[viTriPointSoSanh], pointCu[i]);
            //80km/h = 22m/s
            if (!namTrong)
            {
                if (!pointMoi.Contains(pointCu[i - 1]))
                {
                    pointMoi.Add(pointCu[i - 1]);
                }

                pointMoi.Add(pointCu[i]);
                pointCu[viTriPointSoSanh].thoigianketthuc = pointCu[i - 1].thoigianbantin;
                viTriPointSoSanh = i;
            }
        }
        return pointMoi;
    }

    int coTrungDiem(List<Point> DS)
    {
        if(DS.Count<1)
            return 0;
        Point pcu=DS[0];
        for (int i = 1; i < DS.Count; i++)
            if (pcu.Time == DS[i].Time)// || pcu.thoigianketthuc == DS[i].thoigianketthuc)
                return i;
            else
                pcu = DS[i];
        return 0;
    }
    //public List<Point> FilterAnhTrungNangCapTest(List<Point> pointCu , int MinKhoangCach, int ThoiGianLayBanTin)
    //{
    //    return FilterAnhTrungNangCap1(pointCu, MinKhoangCach, ThoiGianLayBanTin);
    //    //List<Point> DSCanTim = FilterAnhTrungNangCap1(pointCu, MinKhoangCach, ThoiGianLayBanTin);
    //    List<Point> DSCanTim = pointCu;
    //    int i = coTrungDiem(DSCanTim);
    //    if (i>0)
    //    {
    //        Point p1 = DSCanTim[i - 1], p2 = DSCanTim[i], p3 = DSCanTim[i+1];
    //        DSCanTim.Clear();
    //        DSCanTim.Add(p1);
    //        DSCanTim.Add(p2);
    //        DSCanTim.Add(p3);
    //    }
    //    return DSCanTim;
    //}

    Point TinhDiemTrungBinh(List<Point> DSDiem)
    {
        //Double bkMax=0, bkMin=40000000;
        Point diemG=new Point();
        if (DSDiem.Count < 1)
        {
            diemG.Lat = 0;
            diemG.Lng = 0;
            diemG.accuracy = 0;
        }
        else
        {
            double Lat, Lng, accuracy, sumLat = 0, sumLng = 0, sumAccuracy = 0;
            foreach (Point p in DSDiem)
            {
                sumLat += p.Lat;
                sumLng += p.Lng;
                sumAccuracy += p.accuracy;
                //if (p.accuracy>bkMax)
                //    bkMax = p.accuracy;
                //if (p.accuracy < bkMin)
                //    bkMin = p.accuracy;
            }
            diemG.Lat = sumLat/DSDiem.Count;
            diemG.Lng = sumLng / DSDiem.Count;
            diemG.accuracy = sumAccuracy / DSDiem.Count;
            //diemG.accuracy = bkMax;
        }
        return diemG;
    }

    double tinhBanKinhDungIm(Point p,double maxBanKinhDungIm)
    {
        double r3=p.accuracy*5;
        if (r3 > maxBanKinhDungIm)
            return maxBanKinhDungIm;
        else
            return r3;
    }
    public bool KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(Point p1, Point p2)
    {
        bool check = false;
        double kc = GetDistance(p1, p2);
        if (p2.accuracy > kc 
            //+ p1.accuracy * 0 / 4
           )   
        {
            check = true;
        }
        return check;
    }

    double tinhVanTocKm(Point diemBatDau, Point diemKetThuc) //điểm bắt đầu phải là điểm trước, điểm kết thúc phải là điểm sau
    {
        //5-4 : trường sửa diemKetThuc.thoigianbantin => diemKetThuc.thoigianketthuc
        Double gioChenhLech = ((diemKetThuc.thoigianketthuc.Year > 1900 ? diemKetThuc.thoigianketthuc : diemKetThuc.thoigianbantin) - (diemBatDau.thoigianketthuc.Year > 1900 ? diemBatDau.thoigianketthuc : diemBatDau.thoigianbantin)).TotalMilliseconds / 3600000d;
        //Double gioChenhLech = (diemKetThuc.thoigianbantin - diemBatDau.thoigianketthuc).TotalMilliseconds / 3600000d;
        if(gioChenhLech>0)
            return GetDistance(diemBatDau, diemKetThuc) / 1000d / gioChenhLech;
        else
            return -1;
    }
    double tinhVanTocKm_truMaxAccuracy(Point diemBatDau, Point diemKetThuc) //điểm bắt đầu phải là điểm trước, điểm kết thúc phải là điểm sau
    {
        //5-4 : trường sửa diemKetThuc.thoigianbantin => diemKetThuc.thoigianketthuc
        Double gioChenhLech = ((diemKetThuc.thoigianketthuc.Year > 1900 ? diemKetThuc.thoigianketthuc : diemKetThuc.thoigianbantin) - diemBatDau.thoigianketthuc).TotalMilliseconds / 3600000d;
       
        //Double gioChenhLech = (diemKetThuc.thoigianketthuc - diemBatDau.thoigianketthuc).TotalMilliseconds / 3600000d;
        if (gioChenhLech > 0)
            return (GetDistance(diemBatDau, diemKetThuc)  -   diemKetThuc.max_accuracy  -  diemBatDau.accuracy  ) / 1000d / gioChenhLech;
        else
            return -1;
    }
    double tinhVanTocKm_thoigianbantin(Point diemBatDau, Point diemKetThuc) //điểm bắt đầu phải là điểm trước, điểm kết thúc phải là điểm sau
    {
        Double gioChenhLech = (diemKetThuc.thoigianbantin - diemBatDau.thoigianbantin).TotalMilliseconds / 3600000d;
        if (gioChenhLech > 0)
            return GetDistance(diemBatDau, diemKetThuc) / 1000d / gioChenhLech;
        else
            return -1;
    }

    double tinhVanTocKm_WithAccuracy(Point diemBatDau, Point diemKetThuc) //điểm bắt đầu phải là điểm trước, điểm kết thúc phải là điểm sau
    {
        //5-4 : trường sửa diemKetThuc.thoigianbantin => diemKetThuc.thoigianketthuc
        Double gioChenhLech = ((diemKetThuc.thoigianketthuc.Year > 1900 ?  diemKetThuc.thoigianketthuc : diemKetThuc.thoigianbantin) - (diemBatDau.thoigianketthuc.Year > 1900 ? diemBatDau.thoigianketthuc : diemBatDau.thoigianbantin)).TotalMilliseconds / 3600000d;
        //Double gioChenhLech = (diemKetThuc.thoigianbantin - diemBatDau.thoigianketthuc).TotalMilliseconds / 3600000d;
        if (gioChenhLech > 0)
            return GetDistance_WithAccuracy(diemBatDau, diemKetThuc) / 1000d / gioChenhLech;
        else
            return -1;
    }
    double tinhVanTocKm_WithAccuracy_thoigianbantin(Point diemBatDau, Point diemKetThuc) //điểm bắt đầu phải là điểm trước, điểm kết thúc phải là điểm sau
    {
        //5-4 : trường sửa diemKetThuc.thoigianbantin => diemKetThuc.thoigianketthuc 
        Double gioChenhLech = ((diemKetThuc.thoigianketthuc.Year > 1900 ? diemKetThuc.thoigianketthuc : diemKetThuc.thoigianbantin) - (diemBatDau.thoigianketthuc.Year > 1900 ? diemBatDau.thoigianketthuc : diemBatDau.thoigianbantin)).TotalMilliseconds / 3600000d;
       // Double gioChenhLech = (diemKetThuc.thoigianbantin - diemBatDau.thoigianbantin).TotalMilliseconds / 3600000d;
        if (gioChenhLech > 0)
            return GetDistance_WithAccuracy(diemBatDau, diemKetThuc) / 1000d / gioChenhLech;
        else
            return -1;
    }

    bool khoangCachHopLe(Point diemBatDau, Point diemKetThuc, double tocDoTangTocGioiHanKmGioTrenGiay)
    {
        if (diemKetThuc.speed - diemBatDau.speed > tocDoTangTocGioiHanKmGioTrenGiay * (diemKetThuc.thoigianbantin - diemBatDau.thoigianketthuc).TotalMilliseconds / 1000d)
            return false;
        else
            return true;
    }
    bool themVaoDSCanTim(List<Point> DSCanTim, Point diemThem, double tocDoTangTocGioiHanKmGioTrenGiay, double tocDoHopLeToiDa)
    {
        //bat dau bat thuong
        if (diemThem.thoigianbantin == new DateTime(2017, 03, 06, 09, 37, 38))
        {
            string s = "";
        }
        //ket thuc bat thuong
        if (diemThem.thoigianbantin == new DateTime(2017,03,06, 10,09,53))
        {
            string s = "";
        }

        if (diemThem.thoigianbantin == new DateTime(2017, 03, 06, 10, 10, 08))
        {
            string s = "";
        }
        //return false;
        //diemThem.speed=100;
        //diemThem.accuracy = 202;
        //diemThem.Type = Point.PointType.Move;
        //DSCanTim.Add(diemThem);
        //return true;
        //Cần khử điểm bất thường trước khi nhét vào danh sách
        if (DSCanTim.Count < 1)
        {
            
            DSCanTim.Add(diemThem);
        }
        else
            if (//true
                tinhVanTocKm_WithAccuracy(DSCanTim[DSCanTim.Count - 1], diemThem) < tocDoHopLeToiDa
                //diemThem.speed < tocDoHopLeToiDa //bo
               &&  khoangCachHopLe(DSCanTim[DSCanTim.Count - 1], diemThem, tocDoTangTocGioiHanKmGioTrenGiay)
                )
        {
          
            tongKM += TinhKhoangCach.CalculateDistance(
                                new Location() { Longitude = diemThem.Lng, Latitude = diemThem.Lat },
                                new Location()
                                {
                                    Longitude = DSCanTim[DSCanTim.Count - 1].Lng,
                                    Latitude = DSCanTim[DSCanTim.Count - 1].Lat
                                });
            DSCanTim.Add(diemThem);
        }
        else
            return false;
                
        return true;
    }
    bool kiemtrahople(Point diemTruoc, Point diemSau, double tocDoTangTocGioiHanKmGioTrenGiay, double tocDoHopLeToiDa)
    {
        //bat dau bat thuong
         
        //return false;
        //diemThem.speed=100;
        //diemThem.accuracy = 202;
        //diemThem.Type = Point.PointType.Move;
        //DSCanTim.Add(diemThem);
        //return true;
        //Cần khử điểm bất thường trước khi nhét vào danh sách
        if (tinhVanTocKm_thoigianbantin(diemTruoc,diemSau) < tocDoTangTocGioiHanKmGioTrenGiay * (diemSau.thoigianbantin - diemTruoc.thoigianbantin).TotalMilliseconds / 1000d
             &&
                tinhVanTocKm_WithAccuracy_thoigianbantin(diemTruoc, diemSau) < tocDoHopLeToiDa
               //diemThem.speed < tocDoHopLeToiDa //bo
               
                )
        {

           
            return true;
        }
        else
            return false;

        
    }
    public List<Point> FilterAnhTrungNangCap(List<Point> pointCu, int MinKhoangCach, int ThoiGianLayBanTin, double tocDoHopLeToiDa)
    {
        List<Point> DSCanTim = new List<Point>();
        List<Point> DSKhu = new List<Point>();
        //pointCu = FilterAnhTrungNangCap2Lan(pointCu, MinKhoangCach);
        //0.	Khởi tạo DSCanTim={}; DSKhu={}; hằng số MinKhoangCach=100 (mét)
        //return pointCu;
        try
        {
           
            double tocDoTangTocGioiHanKmGioTrenGiay = 70;   //cái gì mà vận tốc tăng nhanh hơn 10km/h trong 1 giây thì là bất thường
             tocDoHopLeToiDa = 150;
            //int MinKhoangCach = 300;
            MinKhoangCach = 200;
            double maxBanKinhDungIm = MinKhoangCach;
            double banKinhTruyHoi = 7;
            int soDiemDungImToiThieu = ThoiGianLayBanTin >= 15000 ? 2 : 5 - ThoiGianLayBanTin / 5000;
            //int soDiemDatNguongThoat = 12;
            int soDiemDatNguongThoat = 60000 / ThoiGianLayBanTin;
            soDiemDatNguongThoat = soDiemDatNguongThoat > 5 ? soDiemDatNguongThoat : 6;


            //TRUONGNM : 14/3 : kiểm tra 6 điểm đầu tiền có bất thường không
            int idxbathuong = -1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = i + 1; j < 6; j++)
                {
                    bool bathuong = !kiemtrahople(pointCu[i], pointCu[j], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);
                    if (bathuong)
                    {
                        idxbathuong = i;
                        break;
                    }
                }
                  
            }

                //end kiểm tra 6 điểm đầu bất thường
                for (int i = ( idxbathuong > -1 ? idxbathuong + 1 : 0); i < pointCu.Count;)// i++  1.	Đặt i=m;
            {
                //try
                {
                    DSKhu.Clear();//1.	DSKhu={};
                    DSKhu.Add(pointCu[i]);//2.	Nhét A[i] vào DSKhu
                    int j = i + 1;//3.	Đặt j = i+1;
                        
                    //4.	WHILE (A[j] bị phủ bởi A[i] AND khoảng cách giữa A[j] và A[i] nhỏ hơn MinKhoangCach AND j<=n) 
                    while (j < pointCu.Count && KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[j], pointCu[i])
                    && GetDistance (pointCu[j], pointCu[i]) < MinKhoangCach)
                    {
                        if (pointCu[j].thoigianbantin == new DateTime(2017, 03, 06, 09, 37, 38))
                        {
                            string s = "";
                        }
                        //ket thuc bat thuong
                        if (pointCu[j].thoigianbantin == new DateTime(2017, 03, 06, 10, 09, 53))
                        {
                            string s = "";
                        }

                        DSKhu.Add(pointCu[j]);//1.	Nhét A[j] vào DSKhu. 
                        j++;//2.	j=j+1;
                    }

                    //3.	Tìm điểm có accuracy nhỏ nhất trong DSKhu
                    Point accuracyNhoNhat = DSKhu[0];
                    Point accuracyLonNhat = DSKhu[0];
                    int vtNhoNhat = 0;
                    int vtLonNhat = 0;
                     
                    for (int k = 1; k < DSKhu.Count; k++)
                    {
                         
                        //Neu tim duoc diem co accuracy nho nhat thi giu lai
                        if (DSKhu[k].accuracy < accuracyNhoNhat.accuracy)
                        {
                            vtNhoNhat = k;
                            accuracyNhoNhat = DSKhu[k];
                        }

                        if (DSKhu[k].accuracy > accuracyLonNhat.accuracy)
                        {
                            vtLonNhat = k;
                            accuracyLonNhat = DSKhu[k];
                        }
                    }
                    //TRUONGNM : 14/3 : kiểm tra xem danh sách điểm khử có bị trùng tất cả các điểm không, nếu có lấy thời gian điểm cuối = thời gian điểm đầu
                    //tránh trường hợp bị treo gps tại 1 điểm sau đó, nhẩy đến 1 điểm khác
                    
                    if (DSKhu[vtNhoNhat].Lat == DSKhu[DSKhu.Count - 1].Lat && DSKhu[vtNhoNhat].Lng == DSKhu[DSKhu.Count - 1].Lng)
                    {
                        DSKhu[vtNhoNhat].thoigianbantin = DSKhu[0].thoigianbantin; 
                        DSKhu[vtNhoNhat].thoigianketthuc = DSKhu[DSKhu.Count - 1].thoigianbantin;
                        DSKhu[vtNhoNhat].speed = DSKhu[DSKhu.Count - 1].speed;
                    }
                    else
                    {
                        DSKhu[vtNhoNhat].thoigianbantin = DSKhu[0].thoigianbantin; //1.	TimeTu = thời gian min của DSKhu
                        DSKhu[vtNhoNhat].thoigianketthuc = DSKhu[DSKhu.Count - 1].thoigianbantin;//2.	TimeDen=  đến thời gian max của DSKhu
                        
                    }

                    DSKhu[vtNhoNhat].max_accuracy = accuracyLonNhat.accuracy;
                    //tính speed
                    if (DSCanTim.Count == 0)
                        if (j < pointCu.Count)
                            DSKhu[vtNhoNhat].speed = tinhVanTocKm_truMaxAccuracy(DSKhu[vtNhoNhat], pointCu[j]);   //km/giờ
                        else
                            DSKhu[vtNhoNhat].speed = 0;
                    else
                        //30/3/2017 : TRUONGNM :  Thuật toán phải sửa ở chỗ : tính vận tốc của điểm trước với điểm trong tập khử có accurcy nhỏ nhất => phải lưu thêm tham số là accuray max 
                        //=> khi đó công thức tính vận tốc = khoảng cách giữa điểm cuối cùng điểm đã lưu - accuray max của điểm đứng im - accuracy của điểm còn lại
                        DSKhu[vtNhoNhat].speed = tinhVanTocKm_truMaxAccuracy(DSCanTim[DSCanTim.Count - 1], DSKhu[vtNhoNhat]);


                    //DSCanTim.Add(DSKhu[vtNhoNhat]);//1.	Nhét điểm nhỏ nhất đó vào DSCanTim 
                    //Trước khi nhét vào DSCanTim phải Khử điểm bất thường
                    themVaoDSCanTim(DSCanTim, DSKhu[vtNhoNhat], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);

                    if (j >= pointCu.Count) { return DSCanTim; };//4.	Nếu j>n thì kết thúc thuật toán và trả về kết quả là DSCanTim

                    //5.	WHILE (A[j] phủ điểm cuối cùng của DSCanTim AND Khoảng cách giữa 2 điểm đó < MinKhoangCach AND j<=n AND accuracy của //A[j] >= accuracy của điểm cuối cùng của DSCanTim)
                    int soDiemBaoSau = 0;
                    //Nếu di chuyển ra khỏi vùng đứng yên RaN điểm
                    int raN = 0;

                    double banKinhDungIm = (DSKhu.Count == 1) ? DSKhu[0].accuracy : tinhBanKinhDungIm(TinhDiemTrungBinh(DSKhu), maxBanKinhDungIm);
                    //double banKinhDungIm = maxBanKinhDungIm;
                    while (raN < soDiemDatNguongThoat)
                    {
                        raN = 0;

                        while
                            (j < pointCu.Count &&
                                (
                                    KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(DSCanTim[DSCanTim.Count - 1], pointCu[j])
                                    //&& GetDistance(pointCu[j], pointCu[i]) < MinKhoangCach 
                                   // && GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[j]) < MinKhoangCach // TRUONGNM : bo ngay 13/03 de test truong hop cua Lien
                                    || GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[j]) < banKinhDungIm
                                    && DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu
                                )
                            )
                        {
                            j++;//1.	Tăng j lên 1 
                            soDiemBaoSau++;
                        }

                        if (j + soDiemDatNguongThoat - 1 < pointCu.Count)  //Có thể thoát
                        {
                            for (int cnt = j; cnt < j + soDiemDatNguongThoat; cnt++)
                            {
                                if (cnt < pointCu.Count)
                                    pointCu[cnt].speed = tinhVanTocKm(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]);
                                if (
                                        (cnt < pointCu.Count && khoangCachHopLe(DSCanTim[DSCanTim.Count - 1], pointCu[cnt], tocDoTangTocGioiHanKmGioTrenGiay) &&
                                            !(
                                                KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(DSCanTim[DSCanTim.Count - 1], pointCu[cnt])
                                                //&& GetDistance(pointCu[cnt], pointCu[i]) < MinKhoangCach //bỏ
                                                //&& GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]) < MinKhoangCach //tạm thời comment ngày 11/03 để test trường hợp của LIên
                                                || DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu
                                                && GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]) < banKinhDungIm
                                            )
                                        )
                                   )
                                    raN++;
                                else
                                    break;
                            }
                            if (raN < soDiemDatNguongThoat)
                            {
                                j++;
                                soDiemBaoSau++;
                            }
                            else
                                raN = soDiemDatNguongThoat;  //nhận giá trị để kết thúc vòng lặp
                        }
                        else    //Không thể thoát vì số điểm còn lại ít hơn số có thể thoát
                        {
                            j = pointCu.Count;      //nhận giá trị để kết thúc thuật toán
                            raN = soDiemDatNguongThoat;      //nhận giá trị để kết thúc vòng lặp
                        }

                    }
                    //6.	Cập nhật thời gian TimeDen của điểm cuối cùng trong DSCanTim bằng thời gian của A[j-1]
                    DSCanTim[DSCanTim.Count - 1].thoigianketthuc = pointCu[j - 1].thoigianbantin;
                    if (DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu)
                    {
                        DSCanTim[DSCanTim.Count - 1].speed = 0;
                    }



                    //7.	IF (j>=n)
                    //1.	THEN: Kết thúc thuật toán và trả về kết quả là DSCanTim 
                    if (j >= pointCu.Count)
                    {
                        return DSCanTim;
                    }
                    else
                    {
                        // sau khi thoat khỏi chỗ đứng im, lần ngược lại danh sách để lấy những điểm có khoảng cách đến chỗ đứng im giảm dần
                        List<Point> DSTruyHoi = new List<Point>();
                        DSTruyHoi.Add(pointCu[j]);
                        for (int cnt = 0; cnt < soDiemBaoSau; cnt++)
                        {
                            if (GetDistance(pointCu[j - 1 - cnt], DSCanTim[DSCanTim.Count - 1]) + banKinhTruyHoi < GetDistance(DSTruyHoi[DSTruyHoi.Count - 1], DSCanTim[DSCanTim.Count - 1])
                                //&& GetDistance(pointCu[j-1-cnt],pointCu[j])+banKinhTruyHoi<GetDistance(pointCu[j],DSCanTim[DSCanTim.Count - 1]))
                                && GetDistance(pointCu[j - 1 - cnt], DSTruyHoi[DSTruyHoi.Count - 1]) + banKinhTruyHoi < GetDistance(DSTruyHoi[DSTruyHoi.Count - 1], DSCanTim[DSCanTim.Count - 1]))
                            {
                                DSTruyHoi.Add(pointCu[j - 1 - cnt]);
                                DSCanTim[DSCanTim.Count - 1].thoigianketthuc = pointCu[j - 1 - cnt - 1].thoigianbantin;
                            }
                        }
                        DSTruyHoi.RemoveAt(0);
                        if (DSTruyHoi.Count > 0)
                        {
                            for (int cnt1 = DSTruyHoi.Count - 1; cnt1 >= 0; cnt1--)
                            {
                                DSTruyHoi[cnt1].thoigianketthuc = DSTruyHoi[cnt1].thoigianbantin;
                                DSTruyHoi[cnt1].speed = tinhVanTocKm(DSCanTim[DSCanTim.Count - 1], DSTruyHoi[cnt1]);

                                //DSCanTim.Add(DSTruyHoi[cnt1]);
                                themVaoDSCanTim(DSCanTim, DSTruyHoi[cnt1], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);
                            }
                        }

                        i = j;//2.	ELSE: Đặt i=j sau đó lặp lại từ bước 2
                    }
                }
                //catch (Exception ex)
                //{
                //    log.Error(ex);
                //    //throw ex;
                //    i++;
                //}
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return DSCanTim;
    }
    /// <summary>
    /// Trường update ngày 5/7/2019 : sửa đoạn speed 
    /// </summary>
    /// <param name="pointCu"></param>
    /// <param name="MinKhoangCach"></param>
    /// <param name="ThoiGianLayBanTin"></param>
    /// <param name="tocDoHopLeToiDa"></param>
    /// <returns></returns>
    public List<Point> FilterAnhTrungNangCap_v2(List<Point> pointCu, int MinKhoangCach, int ThoiGianLayBanTin, double tocDoHopLeToiDa)
    {
        List<Point> DSCanTim = new List<Point>();
        List<Point> DSKhu = new List<Point>();
        //pointCu = FilterAnhTrungNangCap2Lan(pointCu, MinKhoangCach);
        //0.	Khởi tạo DSCanTim={}; DSKhu={}; hằng số MinKhoangCach=100 (mét)
        //return pointCu;
        try
        {

            double tocDoTangTocGioiHanKmGioTrenGiay = 70;   //cái gì mà vận tốc tăng nhanh hơn 10km/h trong 1 giây thì là bất thường
            tocDoHopLeToiDa = 150;
            //int MinKhoangCach = 300;
            MinKhoangCach = 200;
            double maxBanKinhDungIm = MinKhoangCach;
            double banKinhTruyHoi = 7;
            int soDiemDungImToiThieu = ThoiGianLayBanTin >= 15000 ? 2 : 5 - ThoiGianLayBanTin / 5000;
            //int soDiemDatNguongThoat = 12;
            int soDiemDatNguongThoat = 60000 / ThoiGianLayBanTin;
            soDiemDatNguongThoat = soDiemDatNguongThoat > 5 ? soDiemDatNguongThoat : 6;


            //TRUONGNM : 14/3 : kiểm tra 6 điểm đầu tiền có bất thường không
            int idxbathuong = -1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = i + 1; j < 6; j++)
                {
                    bool bathuong = !kiemtrahople(pointCu[i], pointCu[j], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);
                    if (bathuong)
                    {
                        idxbathuong = i;
                        break;
                    }
                }

            }

            //end kiểm tra 6 điểm đầu bất thường
            for (int i = (idxbathuong > -1 ? idxbathuong + 1 : 0); i < pointCu.Count;)// i++  1.	Đặt i=m;
            {
                //try
                {
                    DSKhu.Clear();//1.	DSKhu={};
                    DSKhu.Add(pointCu[i]);//2.	Nhét A[i] vào DSKhu
                    int j = i + 1;//3.	Đặt j = i+1;

                    //4.	WHILE (A[j] bị phủ bởi A[i] AND khoảng cách giữa A[j] và A[i] nhỏ hơn MinKhoangCach AND j<=n) 
                    while (j < pointCu.Count && KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[j], pointCu[i])
                    && GetDistance(pointCu[j], pointCu[i]) < MinKhoangCach)
                    {
                        if (pointCu[j].thoigianbantin == new DateTime(2017, 03, 06, 09, 37, 38))
                        {
                            string s = "";
                        }
                        //ket thuc bat thuong
                        if (pointCu[j].thoigianbantin == new DateTime(2017, 03, 06, 10, 09, 53))
                        {
                            string s = "";
                        }

                        DSKhu.Add(pointCu[j]);//1.	Nhét A[j] vào DSKhu. 
                        j++;//2.	j=j+1;
                    }

                    //3.	Tìm điểm có accuracy nhỏ nhất trong DSKhu
                    Point accuracyNhoNhat = DSKhu[0];
                    Point accuracyLonNhat = DSKhu[0];
                    int vtNhoNhat = 0;
                    int vtLonNhat = 0;

                    for (int k = 1; k < DSKhu.Count; k++)
                    {

                        //Neu tim duoc diem co accuracy nho nhat thi giu lai
                        if (DSKhu[k].accuracy < accuracyNhoNhat.accuracy)
                        {
                            vtNhoNhat = k;
                            accuracyNhoNhat = DSKhu[k];
                        }

                        if (DSKhu[k].accuracy > accuracyLonNhat.accuracy)
                        {
                            vtLonNhat = k;
                            accuracyLonNhat = DSKhu[k];
                        }
                    }
                    //TRUONGNM : 14/3 : kiểm tra xem danh sách điểm khử có bị trùng tất cả các điểm không, nếu có lấy thời gian điểm cuối = thời gian điểm đầu
                    //tránh trường hợp bị treo gps tại 1 điểm sau đó, nhẩy đến 1 điểm khác

                    if (DSKhu[vtNhoNhat].Lat == DSKhu[DSKhu.Count - 1].Lat && DSKhu[vtNhoNhat].Lng == DSKhu[DSKhu.Count - 1].Lng)
                    {
                        DSKhu[vtNhoNhat].thoigianbantin = DSKhu[0].thoigianbantin;
                        DSKhu[vtNhoNhat].thoigianketthuc = DSKhu[DSKhu.Count - 1].thoigianbantin;
                        DSKhu[vtNhoNhat].speed = DSKhu[DSKhu.Count - 1].speed;
                    }
                    else
                    {
                        DSKhu[vtNhoNhat].thoigianbantin = DSKhu[0].thoigianbantin; //1.	TimeTu = thời gian min của DSKhu
                        DSKhu[vtNhoNhat].thoigianketthuc = DSKhu[DSKhu.Count - 1].thoigianbantin;//2.	TimeDen=  đến thời gian max của DSKhu

                    }

                    DSKhu[vtNhoNhat].max_accuracy = accuracyLonNhat.accuracy;
                    //tính speed

                    //khogn can tinh speed do ban tin toa do da co
                    //if (DSCanTim.Count == 0)
                    //    if (j < pointCu.Count)
                    //        DSKhu[vtNhoNhat].speed = tinhVanTocKm_truMaxAccuracy(DSKhu[vtNhoNhat], pointCu[j]);   //km/giờ
                    //    else
                    //        DSKhu[vtNhoNhat].speed = 0;
                    //else
                    //    //30/3/2017 : TRUONGNM :  Thuật toán phải sửa ở chỗ : tính vận tốc của điểm trước với điểm trong tập khử có accurcy nhỏ nhất => phải lưu thêm tham số là accuray max 
                    //    //=> khi đó công thức tính vận tốc = khoảng cách giữa điểm cuối cùng điểm đã lưu - accuray max của điểm đứng im - accuracy của điểm còn lại
                    //    DSKhu[vtNhoNhat].speed = tinhVanTocKm_truMaxAccuracy(DSCanTim[DSCanTim.Count - 1], DSKhu[vtNhoNhat]);


                    //DSCanTim.Add(DSKhu[vtNhoNhat]);//1.	Nhét điểm nhỏ nhất đó vào DSCanTim 
                    //Trước khi nhét vào DSCanTim phải Khử điểm bất thường
                    themVaoDSCanTim(DSCanTim, DSKhu[vtNhoNhat], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);

                    if (j >= pointCu.Count) { return DSCanTim; };//4.	Nếu j>n thì kết thúc thuật toán và trả về kết quả là DSCanTim

                    //5.	WHILE (A[j] phủ điểm cuối cùng của DSCanTim AND Khoảng cách giữa 2 điểm đó < MinKhoangCach AND j<=n AND accuracy của //A[j] >= accuracy của điểm cuối cùng của DSCanTim)
                    int soDiemBaoSau = 0;
                    //Nếu di chuyển ra khỏi vùng đứng yên RaN điểm
                    int raN = 0;

                    double banKinhDungIm = (DSKhu.Count == 1) ? DSKhu[0].accuracy : tinhBanKinhDungIm(TinhDiemTrungBinh(DSKhu), maxBanKinhDungIm);
                    //double banKinhDungIm = maxBanKinhDungIm;
                    while (raN < soDiemDatNguongThoat)
                    {
                        raN = 0;

                        while
                            (j < pointCu.Count &&
                                (
                                    KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(DSCanTim[DSCanTim.Count - 1], pointCu[j])
                                    //&& GetDistance(pointCu[j], pointCu[i]) < MinKhoangCach 
                                    // && GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[j]) < MinKhoangCach // TRUONGNM : bo ngay 13/03 de test truong hop cua Lien
                                    || GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[j]) < banKinhDungIm
                                    && DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu
                                )
                            )
                        {
                            j++;//1.	Tăng j lên 1 
                            soDiemBaoSau++;
                        }

                        if (j + soDiemDatNguongThoat - 1 < pointCu.Count)  //Có thể thoát
                        {
                            for (int cnt = j; cnt < j + soDiemDatNguongThoat; cnt++)
                            {
                                //if (cnt < pointCu.Count)
                                  
                                // pointCu[cnt].speed = tinhVanTocKm(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]);
                                if (
                                        (cnt < pointCu.Count && khoangCachHopLe(DSCanTim[DSCanTim.Count - 1], pointCu[cnt], tocDoTangTocGioiHanKmGioTrenGiay) &&
                                            !(
                                                KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(DSCanTim[DSCanTim.Count - 1], pointCu[cnt])
                                                //&& GetDistance(pointCu[cnt], pointCu[i]) < MinKhoangCach //bỏ
                                                //&& GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]) < MinKhoangCach //tạm thời comment ngày 11/03 để test trường hợp của LIên
                                                || DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu
                                                && GetDistance(DSCanTim[DSCanTim.Count - 1], pointCu[cnt]) < banKinhDungIm
                                            )
                                        )
                                   )
                                    raN++;
                                else
                                    break;
                            }
                            if (raN < soDiemDatNguongThoat)
                            {
                                j++;
                                soDiemBaoSau++;
                            }
                            else
                                raN = soDiemDatNguongThoat;  //nhận giá trị để kết thúc vòng lặp
                        }
                        else    //Không thể thoát vì số điểm còn lại ít hơn số có thể thoát
                        {
                            j = pointCu.Count;      //nhận giá trị để kết thúc thuật toán
                            raN = soDiemDatNguongThoat;      //nhận giá trị để kết thúc vòng lặp
                        }

                    }
                    //6.	Cập nhật thời gian TimeDen của điểm cuối cùng trong DSCanTim bằng thời gian của A[j-1]
                    DSCanTim[DSCanTim.Count - 1].thoigianketthuc = pointCu[j - 1].thoigianbantin;
                    if (DSKhu.Count + soDiemBaoSau >= soDiemDungImToiThieu)
                    {
                        DSCanTim[DSCanTim.Count - 1].speed = 0;
                    }



                    //7.	IF (j>=n)
                    //1.	THEN: Kết thúc thuật toán và trả về kết quả là DSCanTim 
                    if (j >= pointCu.Count)
                    {
                        return DSCanTim;
                    }
                    else
                    {
                        // sau khi thoat khỏi chỗ đứng im, lần ngược lại danh sách để lấy những điểm có khoảng cách đến chỗ đứng im giảm dần
                        List<Point> DSTruyHoi = new List<Point>();
                        DSTruyHoi.Add(pointCu[j]);
                        for (int cnt = 0; cnt < soDiemBaoSau; cnt++)
                        {
                            if (GetDistance(pointCu[j - 1 - cnt], DSCanTim[DSCanTim.Count - 1]) + banKinhTruyHoi < GetDistance(DSTruyHoi[DSTruyHoi.Count - 1], DSCanTim[DSCanTim.Count - 1])
                                //&& GetDistance(pointCu[j-1-cnt],pointCu[j])+banKinhTruyHoi<GetDistance(pointCu[j],DSCanTim[DSCanTim.Count - 1]))
                                && GetDistance(pointCu[j - 1 - cnt], DSTruyHoi[DSTruyHoi.Count - 1]) + banKinhTruyHoi < GetDistance(DSTruyHoi[DSTruyHoi.Count - 1], DSCanTim[DSCanTim.Count - 1]))
                            {
                                DSTruyHoi.Add(pointCu[j - 1 - cnt]);
                                DSCanTim[DSCanTim.Count - 1].thoigianketthuc = pointCu[j - 1 - cnt - 1].thoigianbantin;
                            }
                        }
                        DSTruyHoi.RemoveAt(0);
                        if (DSTruyHoi.Count > 0)
                        {
                            for (int cnt1 = DSTruyHoi.Count - 1; cnt1 >= 0; cnt1--)
                            {
                                DSTruyHoi[cnt1].thoigianketthuc = DSTruyHoi[cnt1].thoigianbantin;
                                //DSTruyHoi[cnt1].speed = tinhVanTocKm(DSCanTim[DSCanTim.Count - 1], DSTruyHoi[cnt1]);

                                //DSCanTim.Add(DSTruyHoi[cnt1]);
                                themVaoDSCanTim(DSCanTim, DSTruyHoi[cnt1], tocDoTangTocGioiHanKmGioTrenGiay, tocDoHopLeToiDa);
                            }
                        }

                        i = j;//2.	ELSE: Đặt i=j sau đó lặp lại từ bước 2
                    }
                }
                //catch (Exception ex)
                //{
                //    log.Error(ex);
                //    //throw ex;
                //    i++;
                //}
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return DSCanTim;
    }
    public List<Point> FilterAnhTrung(List<Point> pointCu)
	{
			List<Point> CanLoaiBo = new List<Point>();
			List<Point> CoTheLoaiBo = new List<Point>();
			
			for(int i=1; i < pointCu.Count - 1; i++)
			{

                try
                {
                    CoTheLoaiBo.Add(pointCu[i - 1]);//2.	Nhét A[i] vào DSK

                    //3.	Nếu A[i] bao A[i+1] và A[i+1] bao A[i] nhét A[i+1] vào DSK
                    //Tăng i lên 1, sau đó lặp lại bước 2 cho đến khi không thể nhét thêm phẩn tử vào DSK
                    if (KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[i], pointCu[i - 1])
                    && KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[i - 1], pointCu[i])
                    )
                    {
                        CoTheLoaiBo.Add(pointCu[i]);
                        continue;
                    }

                    //4.	Trong DSK, giả sử có x phần tử, và phần tử đầu tiên là DSK[0]:
                    //	1.	Nếu x=1 thì kết thúc

                    //Neu chi co 1 diem thi ko loai bo, ket thuc
                    if (CoTheLoaiBo.Count == 1)
                    {
                        CoTheLoaiBo.Clear();
                        continue;
                    }
                    //	2.	Đặt k=0
                    //		1.	Cho j chạy từ k+1 đến x-1, nếu tìm thấy j đầu tiên sao cho DSK[k] không bao DSK[j], thì loại bỏ tất cả các điểm từ DSK[j] đến DSK[x-1] ra khỏi danh sách DSK.
                    //		3.	Tăng k lên 1 rồi Lặp lại bước 4.2, cho đến khi k bằng số phần tử còn lại trong DSK	
                    int vtCheckTiepTheoTrongPointCu = i;
                    for (int k = 0; k < CoTheLoaiBo.Count; k++)
                    {
                        for (int j = k + 1; j < CoTheLoaiBo.Count - 1; j++)
                        {
                            if (KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(CoTheLoaiBo[j], CoTheLoaiBo[k]))
                            {
                                //loại bỏ tất cả các điểm từ DSK[j] đến DSK[x-1] ra khỏi danh sách DSK.
                                for (int m = CoTheLoaiBo.Count - 1; m >= j; m--)
                                {
                                    CoTheLoaiBo.RemoveAt(m);
                                    vtCheckTiepTheoTrongPointCu--;
                                }
                            }
                        }
                    }

                    Point accuracyNhoNhat = CoTheLoaiBo[0];
                    int vtNhoNhat = 0;
                    for (int k = 1; k < CoTheLoaiBo.Count; k++)
                    {
                        //Neu tim duoc diem co accuracy nho nhat thi giu lai
                        if (CoTheLoaiBo[k].accuracy < accuracyNhoNhat.accuracy)
                        {
                            vtNhoNhat = k;
                            accuracyNhoNhat = CoTheLoaiBo[k];
                        }

                    }

                    //vtNhoNhat la diem co accuracy nho nhat
                    //tim xem diem accuracy nho nhat so voi diem dau co nho hon X khoang cach khong
                    //Neu nho hon X khoang cach thi save lai ca diem dau
                    Point NhoNhat = CoTheLoaiBo[vtNhoNhat];
                    //remove diem vtNhoNhat khoi CoTheLoaiBo vi diem nay ko loai bo duoc
                    CoTheLoaiBo.RemoveAt(vtNhoNhat);
                    if (CoTheLoaiBo.Count > 1 && GetDistance(NhoNhat, CoTheLoaiBo[0]) > 300)
                    {
                        CoTheLoaiBo.RemoveAt(0);
                    }

                    CanLoaiBo.AddRange(CoTheLoaiBo);

                    CoTheLoaiBo.Clear();

                    i = vtCheckTiepTheoTrongPointCu;
                }
                catch (Exception ex)
                {
                    log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoTrinhGPSFilter));
                    log.Error(ex);
                }
			}

			//Point moi = Point Cu remove cac diem CanLoaiBo
			foreach(Point p in CanLoaiBo)
			{
				pointCu.Remove(p);
			}
			
			return pointCu;
	}
    public List<Point> FilterAnhTrung_OLD(List<Point> pointCu)
	{
			List<Point> CanLoaiBo = new List<Point>();
			List<Point> CoTheLoaiBo = new List<Point>();
			for(int i=0; i < pointCu.Count; i++)
			{
					if(KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[i], pointCu[i + 1])
					&&KiemTraPointiCoNamTrongPointCuTheoAccuracyKhong(pointCu[i+1], pointCu[i])
					)
					{
						CoTheLoaiBo.Add(pointCu[i]);
						continue;
					}
					if(CoTheLoaiBo.Count==0)
					{
						continue;
					}
					Point accuracyNhoNhat = CoTheLoaiBo[0];
					int vtNhoNhat = 0;
					for(int k = 1; k < CoTheLoaiBo.Count; k++)
					{	
						//Neu tim duoc diem co accuracy nho nhat thi giu lai
						if(CoTheLoaiBo[k].accuracy < accuracyNhoNhat.accuracy)
						{
							vtNhoNhat = k;
							accuracyNhoNhat = CoTheLoaiBo[k];
						}
						
					}
					
					//vtNhoNhat la diem co accuracy nho nhat
					//tim xem diem accuracy nho nhat so voi diem dau co nho hon X khoang cach khong
					//Neu nho hon X khoang cach thi save lai ca diem dau
					Point NhoNhat = CoTheLoaiBo[vtNhoNhat];
					//remove diem vtNhoNhat khoi CoTheLoaiBo vi diem nay ko loai bo duoc
					CoTheLoaiBo.RemoveAt(vtNhoNhat);
					if(GetDistance(NhoNhat, CoTheLoaiBo[0]) < 300)
					{
						CoTheLoaiBo.RemoveAt(0);
					}
					
					CanLoaiBo.AddRange(CoTheLoaiBo);
					
					CoTheLoaiBo.Clear();
			}

			//Point moi = Point Cu remove cac diem CanLoaiBo
			foreach(Point p in CanLoaiBo)
			{
				pointCu.Remove(p);
			}
			
			return pointCu;
	}
  
    public List<Point> FilterKalman(List<Point> pointCu, int tocdomet_giay)
    {
         
        List<Point> DSCanTim = new List<Point>();
         
        
        DateTime epoch = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);
        KalmanLatLong fil = new KalmanLatLong(tocdomet_giay);
        for (int i = 0; i < pointCu.Count; i++)
        {
            if (i == 0 || i == pointCu.Count - 1)
            {
                DSCanTim.Add(pointCu[i]);
            }
            else
            {
                long ms = (long)(pointCu[i].Time - epoch).TotalMilliseconds;
                fil.Process(pointCu[i].Lat, pointCu[i].Lng, (float)pointCu[i].accuracy, ms);
                Point p = new Point();
                p.Lat = fil.get_lat();
                p.Lng = fil.get_lng();
                p.accuracy = fil.get_accuracy();
                p.tennhanvien = pointCu[i].tennhanvien;
                p.idnhanvien = pointCu[i].idnhanvien;
                p.thoigianbantin = epoch.AddMilliseconds(fil.get_TimeStamp());
                DSCanTim.Add(p);
            }
        }
        return DSCanTim;
    }
}


public class KalmanLatLong
{
    private float MinAccuracy = 1;

    private float Q_metres_per_second;
    private long TimeStamp_milliseconds;
    private double lat;
    private double lng;
    private float variance; // P matrix.  Negative means object uninitialised.  NB: units irrelevant, as long as same units used throughout

    public KalmanLatLong(float Q_metres_per_second) { this.Q_metres_per_second = Q_metres_per_second; variance = -1; }

    public long get_TimeStamp() { return TimeStamp_milliseconds; }
    public double get_lat() { return lat; }
    public double get_lng() { return lng; }
    public float get_accuracy() { return (float)Math.Sqrt(variance); }

    public void SetState(double lat, double lng, float accuracy, long TimeStamp_milliseconds)
    {
        this.lat = lat; this.lng = lng; variance = accuracy * accuracy; this.TimeStamp_milliseconds = TimeStamp_milliseconds;
    }

    /// <summary>
    /// Kalman filter processing for lattitude and longitude
    /// </summary>
    /// <param name="lat_measurement_degrees">new measurement of lattidude</param>
    /// <param name="lng_measurement">new measurement of longitude</param>
    /// <param name="accuracy">measurement of 1 standard deviation error in metres</param>
    /// <param name="TimeStamp_milliseconds">time of measurement</param>
    /// <returns>new state</returns>
    public void Process(double lat_measurement, double lng_measurement, float accuracy, long TimeStamp_milliseconds)
    {
        if (accuracy < MinAccuracy) accuracy = MinAccuracy;
        if (variance < 0)
        {
            // if variance < 0, object is unitialised, so initialise with current values
            this.TimeStamp_milliseconds = TimeStamp_milliseconds;
            lat = lat_measurement; lng = lng_measurement; variance = accuracy * accuracy;
        }
        else
        {
            // else apply Kalman filter methodology

            long TimeInc_milliseconds = TimeStamp_milliseconds - this.TimeStamp_milliseconds;
            if (TimeInc_milliseconds > 0)
            {
                // time has moved on, so the uncertainty in the current position increases
                variance += TimeInc_milliseconds * Q_metres_per_second * Q_metres_per_second / 1000;
                this.TimeStamp_milliseconds = TimeStamp_milliseconds;
                // TO DO: USE VELOCITY INFORMATION HERE TO GET A BETTER ESTIMATE OF CURRENT POSITION
            }

            // Kalman gain matrix K = Covarariance * Inverse(Covariance + MeasurementVariance)
            // NB: because K is dimensionless, it doesn't matter that variance has different units to lat and lng
            float K = variance / (variance + accuracy * accuracy);
            // apply K
            lat += K * (lat_measurement - lat);
            lng += K * (lng_measurement - lng);
            // new Covarariance  matrix is (IdentityMatrix - K) * Covarariance 
            variance = (1 - K) * variance;
        }
    }
}

