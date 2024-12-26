import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ApexChart, ApexDataLabels, ApexAxisChartSeries, ApexXAxis, ApexYAxis, ApexTooltip, ApexPlotOptions, ApexResponsive } from 'ng-apexcharts';
import { DashboardService, ToastService } from 'src/services';

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
  selector: 'app-bar-chart',
  standalone: true,
  imports: [NgApexchartsModule],
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.scss'],
})
export class BarChartComponent implements OnInit, AfterViewInit {
  @ViewChild('chart') chart!: any; 
  chartOptions!: Partial<ChartOptions>;
  public chartData: any[] = []; 

  constructor(private dashboardService: DashboardService, private toast: ToastService) {
    this.initializeChartOptions();
  }

  ngOnInit() {
    this.getExpenseListForGraph();
  }

  ngAfterViewInit() {

    setTimeout(() => {
      if (this.chartData.length > 0) {
         this.updateChartOptions();
      }
    }, 100); 
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
    let userId = 0; 
    this.dashboardService.getExpenseForGraph(userId).subscribe({
      next: (response) => {
        if (response && response.data && response.data.length > 0) {
          this.chartData = response.data;
           this.updateChartOptions();
        } else {
          //this.toast.showError('No data available for the graph.');
        }
      },
      error: (errorMessage) => {
        this.toast.showError(errorMessage);
      },
    });
  }

  updateChartOptions() {
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
