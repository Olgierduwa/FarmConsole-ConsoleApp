using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class RuleModel
    {
        public string Name { get; set; }
        public string CaptureType { get; set; }
        public int RequiredLevel { get; set; }
        public bool IsAllowed { get; set; }

        public void Update(int lvl) => IsAllowed = CaptureType == "skill" && RequiredLevel <= lvl ? true : IsAllowed;
        public static RuleModel Find(List<RuleModel> Rules, string _Name)
        {
            foreach (var Rule in Rules)
                if (Rule.Name == _Name) return Rule;
            return null;
        }
        public override string ToString()
        {
            return CaptureType + ", " + RequiredLevel + ", " + IsAllowed + ", " + Name;
        }
    }
}
