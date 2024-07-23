class MButton {
  protected float cx, cy;
  private boolean selected = false;
  
  public MButton(float CX, float CY) {
      cx = CX;
      cy = CY;
  }
    
  public void setText(String v) {
  }
  
  public void setSelected(boolean b) {
      selected = b;
  }
  
  public boolean getSelected() {
      return selected;
  }
  
  public void draw() {
    translate(cx, cy);
  }
  
  public boolean contains(float x, float y) {
      return false;
  }
}

class RectButton extends MButton {
    float w, h;
    String t = "";
  
    public RectButton(float CX, float CY, float W, float H, String text) {
        super(CX, CY);
        w = W;
        h = H;
        t = text;
    }
  
    public void draw() {
        pushMatrix();
        
        super.draw();
        
        strokeWeight(3);
        if (getSelected()) {
            stroke(color(255, 0, 0));
        }
        else {
            stroke(0);
        }
        
        fill(200);
        rect(- w / 2, - h / 2, w, h);
        
        fill(0);
        textAlign(CENTER, CENTER);
        text(t, 0, 0);
        
        popMatrix();
    }
    
    public void setText(String v) {
        t = v;
    }
  
    public boolean contains(float x, float y) {
        return (x >= cx - w / 2) && (x <= cx + w / 2) && (y >= cy - h / 2) && (y <= cy + h / 2);
    }
}

class CircleButton extends MButton {
    float r;
  
    public CircleButton(float CX, float CY, float R) {
        super(CX, CY);
        r = R;
    }
  
    public void draw() {
        pushMatrix();
        
        super.draw();
        stroke(0);
        fill(200);
        circle(0, 0, r);
        
        fill(0);
        if (getSelected()) {
            rect(-0.5 * r, -0.5 * r, 0.25 * r, r);
            rect(0.25 * r, -0.5 * r, 0.25 * r, r);
        }
        else {
          triangle(-0.15 * r, -0.25 * r, -0.15 * r, 0.25 * r, 0.25 * r, 0);
        }
        
        popMatrix();
    }
  
    public boolean contains(float x, float y) {
        return sqrt((cx - x) * (cx - x) + (cy - y) * (cy - y)) <= r;
    }
}
