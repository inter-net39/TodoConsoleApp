using System;

namespace MyApp
{
    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool? AllDayAllDayActivity { get; set; } = false;
        public bool? ActivityRank { get; set; } = false;

        public TaskModel(string description, string startDate, string endDate, bool? activityRank)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
            //
            if (!string.IsNullOrEmpty(endDate))
            {
                EndDate = DateTime.Parse(endDate);
            }
            else
            {
                AllDayAllDayActivity = true;
            }
            ///
            ActivityRank = activityRank;
        }

        public string Export()
        {
            return $"{Description},{Validate.HasValue(StartDate)},{Validate.HasValue(EndDate)},{Validate.HasValue(AllDayAllDayActivity)},{Validate.HasValue(ActivityRank)}";
        }
    }
}