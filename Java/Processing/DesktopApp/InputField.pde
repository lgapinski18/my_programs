class FloatInputField {
    String l = "";
    String t = "";
    boolean selected = false;
    boolean dot = false;
    
    int x, y, w, h;
    
    public FloatInputField(int X, int Y, int W, int H, String label) {
        x = X;
        y = Y;
        w = W;
        h = H;
        l = label;
    }
    
    public void draw() {
        stroke(0);
        strokeWeight(1);
        fill(255);
        rect(x, y, w, h);
        
        fill(0);
        textAlign(LEFT, CENTER);
        text(t, x + (int)(0.1 * w), y + h / 2);
        
        textAlign(LEFT, BOTTOM);
        text(l, x, y - h / 10);
    }
    
    public void setValue(float v) {
        t = "" + v;
    }
    
    public void addDigit(char d) {
        t += d;
    }
    
    public void removeDigit() {
        if (t.length() == 0) {
            return;
        }
        t = t.substring(0, t.length() - 1);
    }
    
    public float getFloat() {
        if (t.equals("")) {
            return 0.0f;
        }
        float f = Float.parseFloat(t);
        
        return f;
    }
    
    public void setSelected(boolean s) {
        selected = s;
    }
    
    public boolean isSelected() {
        return selected;
    }
    
    public boolean isClicked(int X, int Y) {
        return (X >= x) && (X <= (x + w)) && (Y >= y) && (Y <= (y + h));
    }
}
