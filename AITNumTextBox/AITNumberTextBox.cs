using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AITNumTextBox
{
    public class AITNumberTextBox:TextBox
    {
        [DefaultValue("0"),
        Category("Appearance"),
        Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        private int beforePoint = 100;
        private int afterPoint = 100;
        private string oldString = "";
        /// <summary>
        /// Creates a new instance of the NumericTextBox
        /// </summary>
        public AITNumberTextBox()
        {
            TextAlign = HorizontalAlignment.Right;
        }
        /// <summary>
        /// Create Properties to read values  
        /// </summary>
        public int DigitsBeforePoint
        {
            get { return beforePoint; }
            set { beforePoint = value; Invalidate(); }
        }
        public int DigitsAfterPoint
        {
            get { return afterPoint; }
            set { afterPoint = value; Invalidate(); }
        }
        public new HorizontalAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; Invalidate(); }
        }
        /// <summary>
        /// change event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            var inputStr = "";
            var numberConvert = Text.Split(',');
            for (int i = 0; i <= numberConvert.Length - 1; i++)
            {
                inputStr = inputStr + numberConvert[i];
            }
            //validate input
            var regexKey = @"^-{0,1}[\]]*?(\d{0," + beforePoint + "})(\\.\\d{0," + afterPoint + "})?$";
            Regex rx = new Regex(regexKey,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(inputStr);
            if (matches.Count <= 0)
            {
                inputStr = oldString;
            }
            var rightPosition = Text.Length - base.SelectionStart;

            Text = formatNumber(inputStr);
            oldString = Text;
            //set position text cursor
            if (Text.Length > rightPosition)
                base.SelectionStart = Text.Length - rightPosition;
            else
            {
                if (Text.Length != 0)
                {
                    if (Text.Substring(0, 1) == "-")
                    {
                        base.SelectionStart = 1;
                    }
                    else
                        base.SelectionStart = 0;
                }
                else
                    base.SelectionStart = 0;
            }
        }
        //format number
        private string formatNumber(string number)
        {
            var strCut = number;
            var strLast = "";
            var arrStr = strCut.Split('.');
            if (arrStr.Length == 2)
            {
                strLast = "." + arrStr[1];
            }
            var stringCustomDes = "";
            var arrStrRemoveStrik = arrStr[0].Split('-');
            var strRemoveStrik = "";
            var strFist = "";
            if (arrStrRemoveStrik.Length == 1)
                strRemoveStrik = arrStrRemoveStrik[0];
            else
            {
                strRemoveStrik = arrStrRemoveStrik[1];
                strFist = "-";
            }
            if (strRemoveStrik.Length < 4)
            {
                stringCustomDes = strRemoveStrik;
            }
            else
            {
                var countCheckcoman = 0;
                var positionComman = strRemoveStrik.Length % 3;
                var flagCheckcomman = 0;
                for (int i = 0; i <= strRemoveStrik.Length - 1; i++)
                {
                    if (i < positionComman)
                    {
                        stringCustomDes = stringCustomDes + strRemoveStrik[i];
                    }
                    else
                    {
                        if (positionComman != 0 && flagCheckcomman == 0)
                        {
                            stringCustomDes = stringCustomDes + ",";
                            flagCheckcomman = 1;
                        }
                        countCheckcoman++;
                        if (countCheckcoman == 3)
                        {
                            stringCustomDes = stringCustomDes + strRemoveStrik[i];
                            if (i != strRemoveStrik.Length - 1)
                            {
                                stringCustomDes = stringCustomDes + ',';
                                countCheckcoman = 0;
                            }
                        }
                        else
                        {
                            stringCustomDes = stringCustomDes + strRemoveStrik[i];
                        }
                    }
                }
            }
            strFist = strFist + stringCustomDes + strLast;
            return strFist;
        }
    }
}
