import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { BajajChartComponent } from './bajaj-chart/bajaj-chart.component';
import { BarChartComponent } from './bar-chart/bar-chart.component';
import { ChartDataMonthComponent } from './chart-data-month/chart-data-month.component';
import { CommonService, DashboardService, SpinnerService, ToastService } from 'src/services';
import { ApexChart, ApexDataLabels, ApexAxisChartSeries, ApexXAxis, ApexYAxis, ApexTooltip, ApexPlotOptions, ApexResponsive, NgApexchartsModule } from 'ng-apexcharts';
import { ButtonModule } from 'primeng/button';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  responsive: ApexResponsive[];
  xaxis: ApexXAxis;
  colors: string[];
  yaxis: ApexYAxis;
  tooltip: ApexTooltip;
};

@Component({
  selector: 'app-default',
  standalone: true,
  imports: [CommonModule, SharedModule, BajajChartComponent, BarChartComponent, ChartDataMonthComponent, NgApexchartsModule,ButtonModule],
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class DefaultComponent {
  budgetList: any[] = [];
  TransictionList: any[] = [];
  chartOptions!: Partial<ChartOptions>;
  public chartData: any[] = [];
  @ViewChild('chart') chart!: any;
  showAll = false;
  constructor(
    private dashboardservice: DashboardService,
    private toast: ToastService,
    private commonservice: CommonService,
    private spinner:SpinnerService
  ) {
   
  }

  ngOnInit() {
    this.spinner.showSpinner()
    this.getExpenseForCard();
    this.getCategoryTransiction();
    this.getExpenseListForGraph();
  }

  getExpenseForCard() {
    this.spinner.showSpinner()
    let userId = 0;
    this.dashboardservice.getExpenseForCard(userId).subscribe({
      next: (response) => {
        this.budgetList = response.data;
        this.spinner.hideSpinner()
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
        this.spinner.hideSpinner()
      }
    });
  }

  getCategoryTransiction() {
    this.spinner.showSpinner()
    let userId=0
    this.commonservice.getCategoryListForDashboard(userId).subscribe({
      next: (response) => {
        this.TransictionList = response.data;
        this.spinner.hideSpinner()
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
        this.spinner.hideSpinner()
      }
    });
  }

  initializeChartOptions() {
    this.chartOptions = {
      series: [],
      chart: {
        type: 'bar',
        height: 480,
        stacked: true,
        toolbar: {
          show: true,
        },
        background: 'transparent',
      },
      dataLabels: {
        enabled: false,
      },
      colors: ['#1abc9c', '#3498db', '#9b59b6', '#e74c3c'],
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: '20%',
        },
      },
      xaxis: {
        categories: [],
      },
      tooltip: {
        theme: 'light',
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            legend: {
              position: 'bottom',
              offsetX: -10,
              offsetY: 0,
            },
          },
        },
      ],
    };
  }

  getExpenseListForGraph() {
    this.spinner.showSpinner()
    let userId = 0;
    this.dashboardservice.getExpenseForGraph(userId).subscribe({
      next: (response) => {
        if (response && response.data && response.data.length > 0) {
          this.chartData = response.data;
          this.initializeChartOptions();
          this.updateChartOptions();
        } else {
          //this.toast.showError('No data available for the graph.');
        }
        this.spinner.hideSpinner()
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
        this.spinner.hideSpinner()
      },
    });
  }

  updateChartOptions() {
    if (this.chartData.length > 0) {
      const categories = this.chartData.map((item: any) => item.categoryName);
      const seriesData = this.chartData.map((item: any) => item.totalAmount);

      this.chartOptions = {
        ...this.chartOptions,
        xaxis: {
          categories: categories,
        },
        series: [
          {
            name: 'Expenses',
            data: seriesData,
          },
        ],
      };
    }
  }

  ngAfterViewInit() {
    setTimeout(() => {
      if (this.chartData.length > 0) {
        this.updateChartOptions();
      }
    }, 100);
  }
  toggleShowAll() {
    this.showAll = !this.showAll;
  }

  pdfDownload(){
    this.spinner.showSpinner()
   let userId=0
   this.dashboardservice.expensePDFDownload(userId).subscribe({
    next:(response)=>{
    this.downloadFile(response.data?.fileData, response.data?.fileName)
   // this.toast.showError(response.message)
    this.spinner.hideSpinner()
    },
    error:(errorMessage)=>{
     this.toast.showError(errorMessage);
     this.spinner.hideSpinner()
    }
   })
  }

  private downloadFile(fileData: string, fileName: string) {
    const linkSource = `data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,${fileData}`;
    const downloadLink = document.createElement('a');
    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

}
