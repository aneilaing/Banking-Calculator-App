using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

public delegate void NotifyList(double[][] listItemz, int numItemz);
public delegate void NotifyData(double monthlyPayment, double totalInterest);


namespace Banking_Calculator
{

    public partial class LoanPivot1 : PhoneApplicationPage
    {
        double loanAmount;
        double interestRate;
        double loanPeriod;
        bool checker = false;
        double compound;
        string errormsg = "Required";
        string ltype;


        public LoanPivot1()
        {
            InitializeComponent();
        }

        private void LoanPage_Loaded(object sender, RoutedEventArgs e)
        {
            ltype = NavigationContext.QueryString["parameter"];
            if (ltype == "ordinary")
                LoanPivot.Title = "FINANCIAL CALCULATOR: Ordinary Loan";
            if (ltype == "amortized")
            {
                CompoundListPicker.IsEnabled = false;
                LoanPivot.Title = "FINANCIAL CALCULATOR: Amortized Loan";
            }
            CompoundListPicker.SelectedIndex = 0;
        }
        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            bool monthOK = false;
            bool yearOK = false;
            if (LoanAmtTxtBx.Text.Equals("") || LoanAmtTxtBx.Text.Equals("Enter Loan Amount") ||
                LoanAmtTxtBx.Text.Equals(errormsg) || !Double.TryParse(LoanAmtTxtBx.Text, out loanAmount))
            {
                LoanAmtTxtBx.Text = errormsg;
                checker = false;
            }
            else
            {
                loanAmount = Convert.ToDouble(LoanAmtTxtBx.Text);
                checker = true;
            }
            if (InterestRateTxtBx.Text.Equals("") || InterestRateTxtBx.Text.Equals("Enter Interest Rate %") ||
            InterestRateTxtBx.Text.Equals(errormsg) || !Double.TryParse(InterestRateTxtBx.Text, out interestRate))
            {
                InterestRateTxtBx.Text = errormsg;
                checker = false;
            }
            else
            {
                interestRate = Convert.ToDouble(InterestRateTxtBx.Text);
                checker = true;
            }
         
            if (LoanPeriodMthTxtBx.Text.Equals(errormsg) ||LoanPeriodMthTxtBx.Text.Equals("Months") ||
                !Double.TryParse(LoanPeriodMthTxtBx.Text, out loanPeriod)||LoanPeriodMthTxtBx.Text.Equals(""))
            {
                monthOK = false;
            }
            else
            {
                monthOK = true;
            }
            if (LoanPeriodYrTxtBx.Text.Equals(errormsg) || LoanPeriodYrTxtBx.Text.Equals("Years") ||
            !Double.TryParse(LoanPeriodYrTxtBx.Text, out loanPeriod) || LoanPeriodYrTxtBx.Text.Equals(""))
            {
                yearOK = false;
            }
            else
            {
                yearOK = true;
            }
            if(yearOK==false && monthOK==false)
            {
                LoanPeriodMthTxtBx.Text = errormsg;
                LoanPeriodYrTxtBx.Text = errormsg;
                checker = false;
            }
            if(yearOK==true && monthOK==true)
            {
                loanPeriod=0;
                checker = true;
                loanPeriod = loanPeriod + (Convert.ToDouble(LoanPeriodMthTxtBx.Text) /12)+ Convert.ToDouble(LoanPeriodYrTxtBx.Text);
            }
            if(yearOK==true||monthOK==true)
            {
                loanPeriod=0;
                checker = true;
                if (yearOK == true)
                {
                    loanPeriod += Convert.ToDouble(LoanPeriodYrTxtBx.Text);
                    LoanPeriodMthTxtBx.Text = "Months";
                }
                if (monthOK == true)
                {
                    loanPeriod += (Convert.ToDouble(LoanPeriodMthTxtBx.Text)/12);
                    LoanPeriodYrTxtBx.Text = "Years";
                }
            }
            if (CompoundListPicker.SelectedIndex == 0)
                compound = 12;
            if (CompoundListPicker.SelectedIndex == 1)
                compound = 1;
            if(CompoundListPicker.SelectedIndex==2)
                compound=2;
            if (CompoundListPicker.SelectedIndex == 3)
                compound = 4;

            if(checker==true)
            {

              if (ltype == "ordinary")
              {
                  OrdinaryLoan ord1 = new OrdinaryLoan(loanAmount, interestRate, loanPeriod, compound);
                  ord1.CalculateValues();
                  NotifyData ord1Data = new NotifyData(NotifyPopulateData);
                  NotifyList ord1List = new NotifyList(NotifyPopulateList);
                
                      if(ord1Data!=null)
                      ord1Data(ord1.MonthlyPayment, ord1.TotalInterest);
                  if(ord1List!=null)
                      ord1List(ord1.ListItemz, ord1.NumItems);
                  
                  
              }

              if (ltype == "amortized")
              {
                  AmortizedLoan amort1 = new AmortizedLoan(loanAmount, interestRate, loanPeriod, compound);
                  amort1.CalculateValues();
                  NotifyData amort1Data = new NotifyData(NotifyPopulateData);
                  NotifyList amort1List = new NotifyList(NotifyPopulateList);
                  if(amort1Data!=null)
                      amort1Data(amort1.MonthlyPayment, amort1.TotalInterest);
                  if(amort1List!=null)
                      amort1List(amort1.ListItemz, amort1.NumItems);
                 
              }

            }
       }
        private void NotifyPopulateList(double[][] loanInfo, int loanlength)
        {
            ListPageListBox.Items.Add(string.Format("{0,5} {1,13} {2,13} {3,8} {4,8} \n\n", "No.", "Balance", "Principal", "Interest", "Payment"));
            for (int i = 0; i < loanlength; i++)
                ListPageListBox.Items.Add(string.Format("{0,5:0} {1,15:C} {2,15:C} {3,10:C} {4,10:C}\n", loanInfo[i][0], loanInfo[i][1], loanInfo[i][2], loanInfo[i][3], loanInfo[i][4]));
        }
        private void NotifyPopulateData(double monthlyPayment, double totalInterest)
        {
            DispLoanAmtTxtBx1.Text = string.Format("{0:C}", loanAmount);
            DispLoanAmtTxtBx1.TextAlignment = TextAlignment.Right;
            DispIntRateTxtBx1.Text = string.Format("{0} %", interestRate);
            DispIntRateTxtBx1.TextAlignment = TextAlignment.Right;
            DispPeriodTxtBx1.Text = string.Format("{0} years", loanPeriod);
            DispPeriodTxtBx1.TextAlignment = TextAlignment.Right;
            if(ltype=="ordinary")
            DispCompoundTxtBx1.Text = "Monthly";
            if (ltype == "amortized")
            {
                if (compound == 12)
                    DispCompoundTxtBx1.Text = "Monthly";
                if (compound == 1)
                    DispCompoundTxtBx1.Text = "Yearly";
                if (compound == 4)
                    DispCompoundTxtBx1.Text = "Quarterly";
                if (compound == 2)
                    DispCompoundTxtBx1.Text = "Half-Yearly";
            }
            DispCompoundTxtBx1.TextAlignment = TextAlignment.Right;
            DispMonthPmtTxtBx1.Text = string.Format("{0:C}", monthlyPayment);
            DispMonthPmtTxtBx1.TextAlignment = TextAlignment.Right;
            DispTotalIntTxtBx1.Text = string.Format("{0:C}", totalInterest);
            DispTotalIntTxtBx1.TextAlignment = TextAlignment.Right;
            DispTotalAmtTxtBx1.Text = string.Format("{0:C}", loanAmount + totalInterest);
            DispTotalAmtTxtBx1.TextAlignment = TextAlignment.Right;
        }

        private void CompoundListPicker_GotFocus(object sender, RoutedEventArgs e)
        {

            if (CompoundListPicker.ListPickerMode == ListPickerMode.Normal)
                CompoundListPicker.Open();
        }

        private void LoanAmtTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoanAmtTxtBx.Text.Equals("Enter Loan Amount")||LoanAmtTxtBx.Text.Equals(errormsg))
            LoanAmtTxtBx.Text = string.Empty;
        }

        private void LoanAmtTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoanAmtTxtBx.Text.Equals("") || LoanAmtTxtBx.Text.Equals(errormsg))
            LoanAmtTxtBx.Text = "Enter Loan Amount:";
        }

        private void LoanPeriodYrTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if(LoanPeriodYrTxtBx.Text.Equals("Years")||LoanPeriodYrTxtBx.Text.Equals(errormsg))
            LoanPeriodYrTxtBx.Text = string.Empty;
        }

        private void LoanPeriodYrTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if(LoanPeriodYrTxtBx.Text.Equals("")||LoanPeriodYrTxtBx.Text.Equals(errormsg))
            LoanPeriodYrTxtBx.Text = "Years";
        }

        private void LoanPeriodMthTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoanPeriodMthTxtBx.Text.Equals("Months") || LoanPeriodMthTxtBx.Text.Equals(errormsg))
            LoanPeriodMthTxtBx.Text = string.Empty;
        }

        private void LoanPeriodMthTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (LoanPeriodMthTxtBx.Text.Equals("") || LoanPeriodMthTxtBx.Text.Equals(errormsg))
            LoanPeriodMthTxtBx.Text = "Months";
        }

        private void InterestRateTxtBx_GotFocus(object sender, RoutedEventArgs e)
        {
            if (InterestRateTxtBx.Text.Equals("Interest Rate %") || InterestRateTxtBx.Text.Equals(errormsg))
            InterestRateTxtBx.Text = string.Empty;
        }

        private void InterestRateTxtBx_LostFocus(object sender, RoutedEventArgs e)
        {
            if (InterestRateTxtBx.Text.Equals("") || InterestRateTxtBx.Text.Equals(errormsg))
            InterestRateTxtBx.Text = "Interest Rate %";
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            LoanAmtTxtBx.Text ="Enter Loan Amount";
            InterestRateTxtBx.Text = "Enter Interest Rate %";
            LoanPeriodMthTxtBx.Text = "Months";
            LoanPeriodYrTxtBx.Text = "Years";
            CompoundListPicker.SelectedIndex = 0;
        }

    } 
    }

abstract class Loans
{
      public abstract double TotalInterest
      {
          get;
      }
           public abstract double MonthlyPayment
           {
               get;
           }
                public abstract double[][] ListItemz
                {
                    get;
                }
                public abstract int NumItems
                {
                    get;
                }
    public abstract void CalculateValues();
}

class OrdinaryLoan: Loans
{
    double oLoanAmount;
    double oLoanInterest;
    double oLoanPeriod;
    double ocompound;
    double totalInterest;
    double monthlyPayment;
    double [][] loanList;
    int listNum;


    public OrdinaryLoan(double oLoanAmount, double oLoanInterest, double oLoanPeriod, double ocompound)
    {
        
        this.oLoanAmount = oLoanAmount;
        this.oLoanInterest = oLoanInterest;
        this.oLoanPeriod = oLoanPeriod;
        this.ocompound = ocompound;
    }

    public override double TotalInterest
    {
        get { return this.totalInterest; }
    }
    public override double MonthlyPayment
    {
        get { return this.monthlyPayment; }
    }
    public override double[][] ListItemz
    {
        get { return this.loanList; }
    }
    public override int NumItems
    {
        get { return this.listNum; }
    }
    public override void CalculateValues()
    {

        double newBalance = oLoanAmount;
        double totalnumpayments = ocompound * oLoanPeriod;
        int loanlength = Convert.ToInt32(Math.Ceiling(totalnumpayments));
        loanList = new double[loanlength][];
        monthlyPayment = oLoanAmount / totalnumpayments;
        totalInterest=0;
        for (int i = 0; i < loanlength; i++)
        {
            loanList[i] = new double[5];
            loanList[i][0] = Convert.ToDouble(i + 1);
            double interest = newBalance * (oLoanInterest / 100 / 12);
            loanList[i][1] = newBalance-monthlyPayment;
            loanList[i][2] = monthlyPayment;
            loanList[i][3] = interest;
            loanList[i][4] = monthlyPayment+interest;
            newBalance = newBalance - monthlyPayment ;
            totalInterest += interest;
        }
        listNum = loanList.Length;
        
    }

}

class AmortizedLoan : Loans
{
    double aLoanAmount;
    double aLoanInterest;
    double aLoanPeriod;
    double acompound;
    double totalInterest;
    double monthlyPayment;
    double [][] loanList;
    int listNum;


    public AmortizedLoan(double aLoanAmount, double aLoanInterest, double aLoanPeriod, double acompound)//acpmpound is always equal to 1 in this version of the amortized loan
    {
        
        this.aLoanAmount = aLoanAmount;
        this.aLoanInterest = aLoanInterest;
        this.aLoanPeriod = aLoanPeriod;
        this.acompound = acompound;
    }

    public override double TotalInterest
    {
        get { return this.totalInterest; }
    }
    public override double MonthlyPayment
    {
        get { return this.monthlyPayment; }
    }
    public override double[][] ListItemz
    {
        get { return this.loanList; }
    }
    public override int NumItems
    {
        get { return this.listNum; }
    }
    public override void CalculateValues()
    {
        double newBalance = aLoanAmount;
        double periodicInterestRate = aLoanInterest/acompound/100;
        double totalnumpayments = acompound * aLoanPeriod;
        int loanlength = Convert.ToInt32(Math.Ceiling(totalnumpayments));
        loanList = new double[loanlength][];
        double shorten = Math.Pow(1+periodicInterestRate, totalnumpayments);
        monthlyPayment = aLoanAmount * ((periodicInterestRate * shorten) / (shorten - 1));
        //monthlyPayment = (periodicInterestRate*aLoanAmount*Math.Pow((1+periodicInterestRate), aLoanPeriod))/(Math.Pow((1+periodicInterestRate), aLoanPeriod)-1);
        for (int i = 0; i < loanlength; i++)
        {
            double interest = newBalance * periodicInterestRate;
            newBalance = newBalance - monthlyPayment+interest;
            loanList[i] = new double[5];
            loanList[i][0] = Convert.ToDouble(i + 1);
            loanList[i][1] = newBalance;
            loanList[i][2] = monthlyPayment - interest;
            loanList[i][3] = interest;
            loanList[i][4] = monthlyPayment;
            
            totalInterest += interest;
        }
        listNum = loanList.Length;
    }
}