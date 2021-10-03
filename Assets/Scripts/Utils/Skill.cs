namespace Utils
{
    public class Skill
    {
        private string _name;
        private string _targetTag;
        private float _unstabilityCost;
        private float _skillDuration;

        public Skill(string name, string targetTag, float unstabilityCost, float skillDuration)
        {
            _name = name;
            _targetTag = targetTag;
            _unstabilityCost = unstabilityCost;
            _skillDuration = skillDuration;
        }

        public string TargetTag => _targetTag;

        public string Name => _name;
        
        public float UnstabilityCost => _unstabilityCost;

        public float SkillDuration => _skillDuration;
    }
}