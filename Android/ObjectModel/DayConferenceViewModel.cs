using Android.Runtime;

namespace Conf.ObjectModel
{
    [Preserve]
    public class DayConferenceViewModel
    {
        public int SortOrder { get; set; }
        public string Section { get; set; }
        public string Day { get; set; }
        public string ConfCode { get; set; }
        public string SessCode { get; set; }

        private string _lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                }
            }
        }

        private string _lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                }
            }
        }
    }
}