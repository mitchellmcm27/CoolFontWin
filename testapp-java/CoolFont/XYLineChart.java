package CoolFont;

import javax.swing.JFrame;
import javax.swing.JPanel;

import java.awt.BorderLayout;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.JFreeChart;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;

public class XYLineChart extends JFrame {

  XYSeriesCollection dataset = new XYSeriesCollection();
  // default labels
  private String xAxisLabel = "x";
  private String yAxisLabel = "t";
  private String chartTitle = " ";

  public XYLineChart(String title, XYSeries series) {
    super(title);
    JPanel chartPanel = createChartPanel(series);
    add(chartPanel,BorderLayout.CENTER);
    setSize(640, 480);
    setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    setLocationRelativeTo(null);
  }

  private JPanel createChartPanel(XYSeries series) {
    dataset.addSeries(series);
    JFreeChart chart = ChartFactory.createXYLineChart(chartTitle,
            xAxisLabel, yAxisLabel, dataset);
    return new ChartPanel(chart);
  }
}