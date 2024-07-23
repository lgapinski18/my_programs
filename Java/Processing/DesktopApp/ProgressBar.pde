class ProgressBar {
  int bx, by, ex, ey;
  int value = 0;
  int totalValue = 100;
  int r;
  
  color preColor = color(50);
  color postColor = color(200);
  
  public ProgressBar(int BX, int BY, int EX, int EY, int R) {
      bx = BX;
      by = BY;
      ex = EX;
      ey = EY;
      r = R;
  }
  
  public void setPreColor(color c) {
      preColor = c;
  }
  
  public void setPostColor(color c) {
      postColor = c;
  }
  
  public void setValue(int v) {
      value = v;
  }
  
  public void setTotalValue(int v) {
      totalValue = v;
  }
  
  public void draw() {
      int px = bx + (ex - bx) * value / totalValue;
      int py = by + (ey - by) * value / totalValue;
      
      strokeWeight(5);
      stroke(postColor);
      line(px, py, ex, ey);
      stroke(preColor);
      line(bx, by, px, py);
      
      strokeWeight(0);
      circle(px, py, 2 * r);
  }
  
  public boolean contains(int x, int y) {
      return (x >= bx) && (x <= ex) && (y >= (by - r)) && (y <= (by + r)); 
  }
  
  public int drag(int x, int y) {
      value = totalValue * (x - bx) / (ex - bx);
      
      if (value > totalValue) {
          value = totalValue;
      }
      else if (value < 0) {
          value = 0;
      }
      
      return value;
  }
}
