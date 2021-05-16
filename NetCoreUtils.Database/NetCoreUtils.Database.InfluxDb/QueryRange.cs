namespace NetCoreUtils.Database.InfluxDb
{
    public class QueryRange
    {
        int _number;
        RangeUnit _unit;

        public QueryRange(int number, RangeUnit unit)
        {
            _number = number;
            _unit = unit;
        }

        public string ToFluxString()
        {
            switch (_unit)
            {
                case RangeUnit.minute:
                    return $"{_number}m";
                case RangeUnit.hour:
                    return $"{_number}h";
                case RangeUnit.day:
                    return $"{_number}d";
            }
            return "-1h"; // default return last 1 hour
        }
    }
}
