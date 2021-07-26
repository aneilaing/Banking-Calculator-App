using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;



namespace Banking_Calculator
{
    public delegate void NotifyList(double[][] listItemz, int numItemz);
    public delegate void NotifyData(double accruedAmount, double totalInterest);
    public partial class DepositPivot1 : PhoneApplicationPage
    {
        string dtype;
        double depositAmount;
        double depositInterest;
        double depositPeriod;
        double compound;
        public DepositPivot1()
        {
            InitializeComponent();
          
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            dtype = NavigationContext.QueryString["parameter"];
            if (dtype == "simple")
            {
                DepositControl1.EnableCompound = true;
                DepositPivot.Title = "FINANCIAL CALCULATOR: Simple Interest";
            }
            if (dtype == "compoundi")
            {
                DepositControl1.EnableCompound = false;
                DepositPivot.Title = "FINANCIAL CALCULATOR: Compound Interest";
            }
        }
        private void DepositControl_CalculateIt(object sender, EventArgs e)
        {
            this.depositAmount = DepositControl1.StartAmount;
            this.depositInterest = DepositControl1.InterestRate;
            this.depositPeriod = DepositControl1.DepositPeriod;
            this.compound = DepositControl1.Compound;

            if(dtype=="compoundi")
            {
                CompoundInterest ci = new CompoundInterest(depositAmount, depositInterest, depositPeriod, compound);
                ci.CalculateValues();
                NotifyData CompInData = new NotifyData(NotifyPopulateData);
                NotifyList CompInList = new NotifyList(NotifyPopulateList);
                if (CompInData != null)
                    CompInData(ci.AccruedAmount, ci.TotalInterest);
                if(CompInList!=null)
                    CompInList(ci.ListItemz, ci.NumItems);
            }
            if (dtype == "simple")
            {
                SimpleInterest si = new SimpleInterest(depositAmount, depositInterest, depositPeriod);
                si.CalculateValues();
                NotifyData SimpInData = new NotifyData(NotifyPopulateData);
                NotifyList SimpInList = new NotifyList(NotifyPopulateList);
                if (SimpInData != null)
                    SimpInData(si.AccruedAmount,si.TotalInterest);
                if (SimpInList != null)
                    SimpInList(si.ListItemz, si.NumItems);

            }
        }
        private void DepositControl_CalculateNo(object sender, EventArgs e)
        {
            ListPageListBox.Items.Add("Enter the values on the Input page.");
            DispDepositAmtTxtBx1.Text = "No values entered";
        }

        private void NotifyPopulateList(double[][] depositInfo, int depositlength)
        {
            ListPageListBox.Items.Add(string.Format("{0,5} {1,15} {2,15} {3,15}\n\n", "No", "Start Amount", "Interest", "End Balance"));
            for (int i = 0; i < depositlength; i++)
                ListPageListBox.Items.Add(string.Format("{0,5:0} {1,17:C} {2,17:C} {3,17:C}\n", depositInfo[i][0], depositInfo[i][1], depositInfo[i][2], depositInfo[i][3]));
        }
        private void NotifyPopulateData(double monthlyPayment, double totalInterest)
        {
            DispDepositAmtTxtBx1.Text = string.Format("{0:C}", depositAmount);
            DispDepositAmtTxtBx1.TextAlignment = TextAlignment.Right;
            DispIntRateTxtBx1.Text = string.Format("{0} %", depositInterest);
            DispIntRateTxtBx1.TextAlignment = TextAlignment.Right;
            if(depositPeriod==1)
            DispPeriodTxtBx1.Text = string.Format("{0} year", depositPeriod);
            else
                DispPeriodTxtBx1.Text = string.Format("{0} years", depositPeriod);
            DispPeriodTxtBx1.TextAlignment = TextAlignment.Right;
            if (dtype == "simple")
                DispCompoundTxtBx1.Text = "No compounding";

            if (dtype == "compoundi")
            {
                if(compound==12)
                DispCompoundTxtBx1.Text = "Monthly";
                if (compound == 1)
                    DispCompoundTxtBx1.Text = "Yearly";
                if (compound == 4)
                    DispCompoundTxtBx1.Text = "Quarterly";
                if (compound == 2)
                    DispCompoundTxtBx1.Text = "Half-Yearly";
            }
            
            DispCompoundTxtBx1.TextAlignment = TextAlignment.Right;
            DispTotalIntTxtBx1.Text = string.Format("{0:C}", totalInterest);
            DispTotalIntTxtBx1.TextAlignment = TextAlignment.Right;
            DispTotalAmtTxtBx1.Text = string.Format("{0:C}", depositAmount + totalInterest);
            DispTotalAmtTxtBx1.TextAlignment = TextAlignment.Right;
        }

    }
}

interface IDeposit
{
    double TotalInterest
    {
        get;
    }
    double AccruedAmount
    {
        get;
    }
    double[][] ListItemz
    {
        get;
    }
    int NumItems
    {
        get;
    }
    void CalculateValues();
}

class CompoundInterest:IDeposit
{
    double startAmount;
    double depositInterest;
    double depositPeriod;
    double depositCompound;
    double totalInterest=0;
    double accruedAmount;
    double [][] depositList;
    int listNum;


    public CompoundInterest(double startAmount, double depositInterest, double depositPeriod, double depositCompound)
    {
        
        this.startAmount = startAmount;
        this.depositInterest = depositInterest;
        this.depositPeriod = depositPeriod;
        this.depositCompound = depositCompound;
    }

    public double TotalInterest
    {
        get { return this.totalInterest; }
    }
    public double AccruedAmount
    {
        get { return this.accruedAmount; }
    }
    public double[][] ListItemz
    {
        get { return this.depositList; }
    }
    public int NumItems
    {
        get { return this.listNum; }
    }
    public void CalculateValues()
    {
        double realInterest = depositInterest/100;
        accruedAmount = startAmount * Math.Pow(1 + (realInterest / depositCompound), depositCompound * depositPeriod);
        listNum = Convert.ToInt32(Math.Ceiling(depositCompound * depositPeriod));
        depositList = new double[listNum][];
        double newBalance = startAmount;
        for(int i=0; i<listNum; i++)
        {
            depositList[i] = new double[4];
            depositList[i][0]=Convert.ToDouble(i+1);
            depositList[i][1]= newBalance;
            double interest = newBalance * realInterest/depositCompound;
            depositList[i][2]= interest;
            newBalance += interest;
            depositList[i][3] = newBalance;
            totalInterest += interest;
        }
    }
}

class SimpleInterest : IDeposit
{
    double startAmount;                     //principal
    double depositInterest;                 //rate
    double depositPeriod;                   //time
    double totalInterest;
    double accruedAmount;
    double[][] depositList;
    int listNum;


    public SimpleInterest(double startAmount, double depositInterest, double depositPeriod)
    {
        this.startAmount = startAmount;
        this.depositInterest = depositInterest;
        this.depositPeriod = depositPeriod;
    }

    public double TotalInterest
    {
        get { return this.totalInterest; }
    }
    public double AccruedAmount
    {
        get { return this.accruedAmount; }
    }
    public double[][] ListItemz
    {
        get { return this.depositList; }
    }
    public int NumItems
    {
        get { return this.listNum; }
    }
    public void CalculateValues()
    {
        accruedAmount = startAmount * (1 + (depositInterest * depositPeriod/100));
        double interest = startAmount * depositInterest*depositPeriod / 100;
        totalInterest = interest;
        depositList = new double[1][];
        depositList[0] = new double[4];
        depositList[0][0] = 1;
        depositList[0][1] = startAmount;
        depositList[0][2]= interest;
        depositList[0][3] = accruedAmount;
        listNum = 1;
    }
}