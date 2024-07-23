class Button {
  private PShape bShape;
  protected float cx, cy;
  
  public Button(PShape BShape, float CX, float CY) {
      bShape = BShape;
      cx = CX;
      cy = CY;
  }
    
  public void setText(String v) {
  }
  
  public void setShape(PShape BShape) {
      bShape = BShape;
  }
  
  public PShape getShape() {
      return bShape;
  }
  
  public void draw() {
    translate(cx, cy);
  }
  
  public boolean contains(float x, float y) {
      return false;
  }
}

class RectButton extends Button {
    float w, h;
    String t = "";
  
    public RectButton(PShape BShape, float CX, float CY, float W, float H, String text) {
        super(BShape, CX, CY);
        w = W;
        h = H;
        t = text;
    }
  
    public void draw() {
        pushMatrix();
        
        super.draw();
        shape(getShape(), - w / 2, - h / 2, w, h);
        
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

class CircleButton extends Button {
    float r;
  
    public CircleButton(PShape BShape, float CX, float CY, float R) {
        super(BShape, CX, CY);
        r = R;
    }
  
    public void draw() {
        pushMatrix();
        
        super.draw();
        shape(getShape(), - r, - r, 2 * r, 2 * r);
        
        popMatrix();
    }
  
    public boolean contains(float x, float y) {
        return sqrt((cx - x) * (cx - x) + (cy - y) * (cy - y)) <= r;
    }
}
