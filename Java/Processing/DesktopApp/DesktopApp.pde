 import processing.sound.*;

SoundFile soundfile;
String fileTitle = "Niewybrano utworu";

PFont font;
PFont font2;

PShape pause;
PShape play;
PShape rateRect;
PShape rateRectR;

Button playButton;
Button rateX05Button;
Button rateX1Button;
Button rateX2Button;
Button openButton;

ProgressBar progressBar;

int duration = 0;
String durationText = "**:**:**";
int timeElapsed = 0;
String elapsedText = "**:**:**";
int lastTime = 0;

float rate = 1; 
int loadingNo = 0;
String[] dots = {"", ".", "..", "..."};

boolean isLoading = false;

FFT fft;
//AudioIn audioIn;
int bands = 16;
float[] spectrum = new float[bands];
color[] colors = {color(0, 255, 0), color(43, 212, 0), color(86, 169, 0), color(129, 129, 0), color(169, 86, 0), color(212, 43, 0), color(255, 0, 0)};
int[] treshholds = {0, 50, 100, 200, 400, 600, 1000};


PVector dPos;
float delayTime = 0.0f;
Delay delay;
boolean dOn = false;
Button dButton;

PVector hpPos;
float hpFreq = 5000;
HighPass highPass;
boolean hpOn = false;
Button hpButton;

PVector lpPos;
float lpFreq = 1000;
LowPass lowPass;
boolean lpOn = false;
Button lpButton;

PVector rPos;
float damp = 0.1;
float room = 0.5;
float wet = 0.7;
Reverb reverb;
boolean rOn = false;
Button rButton;

int selectedIF = -1;
ArrayList<FloatInputField> inputFields = new ArrayList<FloatInputField>();


void setup() {
    size(640, 500);
    
    fft = new FFT(this, bands);
    //audioIn = new AudioIn(this, 0);
    //audioIn.play();
    //fft.input(audioIn);
    
    delay = new Delay(this);
    
    highPass = new HighPass(this);
    
    lowPass = new LowPass(this);
    
    reverb = new Reverb(this);
    
    
    font = createFont("Calibri", 25);
    font2 = createFont("Calibri", 15);
    textFont(font);
    
    pause = loadShape("pause.svg");
    play = loadShape("play.svg");
    
    playButton = new CircleButton(play, width / 2, 0.85 * height, 0.1 * height);
    
    rateRect  = loadShape("rect.svg");
    rateRectR  = loadShape("rectR.svg");
    rateX05Button = new RectButton(rateRect, 0.1 * width, 0.9 * height, 0.1 * width, 0.05 * height, "0.5");
    rateX1Button = new RectButton(rateRectR, 0.2 * width + 10, 0.9 * height, 0.1 * width, 0.05 * height, "1");
    rateX2Button = new RectButton(rateRect, 0.3 * width + 20, 0.9 * height, 0.1 * width, 0.05 * height, "2");
    openButton = new RectButton(rateRect, 0.825 * width, 0.9 * height, 0.25 * width, 0.05 * height, "otwórz mp3");

    progressBar = new ProgressBar((int)(0.05 * width), (int)(0.7 * height), (int)(0.95 * width), (int)(0.7 * height), 10);

    int fw = (int)(0.15 * width);
    int fh = 25;
    
    dPos = new PVector(0.05 * width, 0.1 * height);
    inputFields.add(new FloatInputField((int)dPos.x + 10, (int)dPos.y + 10, fw, fh, "delay time:"));
    inputFields.get(0).setValue(delayTime);
    dButton = new RectButton(rateRect, dPos.x + 10 + fw / 2, dPos.y + 10 + 2 * fh, fw, fh, "Wyłączony");

    hpPos = new PVector(0.05 * width, 0.31 * height);
    inputFields.add(new FloatInputField((int)hpPos.x + 10, (int)hpPos.y + 10, fw, fh, "Częstotliwość:"));
    inputFields.get(1).setValue(hpFreq);
    hpButton = new RectButton(rateRect, hpPos.x + 10 + fw / 2, hpPos.y + 10 + 2 * fh, fw, fh, "Wyłączony");

    lpPos = new PVector(0.95 * width - fw, 0.1 * height);
    inputFields.add(new FloatInputField((int)lpPos.x + 10, (int)lpPos.y + 10, fw, fh, "Częstotliwość:"));
    inputFields.get(2).setValue(lpFreq);
    lpButton = new RectButton(rateRect, lpPos.x + 10 + fw / 2, lpPos.y + 10 + 2 * fh, fw, fh, "Wyłączony");

    rPos = new PVector(0.95 * width - fw, 0.31 * height);
    inputFields.add(new FloatInputField((int)rPos.x + 10, (int)rPos.y + 10, fw, fh, "damp:"));
    inputFields.get(3).setValue(damp);
    inputFields.add(new FloatInputField((int)rPos.x + 10, (int)rPos.y + 10 + 2 * fh, fw, fh, "room:"));
    inputFields.get(4).setValue(room);
    inputFields.add(new FloatInputField((int)rPos.x + 10, (int)rPos.y + 10 + 4 * fh, fw, fh, "wet:"));
    inputFields.get(5).setValue(wet);
    rButton = new RectButton(rateRect, rPos.x + 10 + fw / 2, rPos.y + 10 + 6 * fh, fw, fh, "Wyłączony");
}

void draw() {
    background(255);
    
    if (soundfile != null && soundfile.isPlaying()) {
        int nowTime = millis();
        timeElapsed += (int)((nowTime - lastTime) * rate);
        lastTime = nowTime;
        
        if (timeElapsed > duration) {
            pause();
        }
        else {
            progressBar.setValue(timeElapsed);
            
            int el = timeElapsed / 1000;
            elapsedText = "" + el / 3600 + ":" + (el / 60) % 60 + ":" + el % 60;
        }
    }
    
    fill(0);
    textAlign(LEFT, TOP);
    text(fileTitle, 0.05 * width, 0.02 * height);
    
    text(elapsedText, 0.05 * width, 0.8 * height);
    textAlign(RIGHT, TOP);
    text(durationText, 0.95 * width, 0.8 * height);
    
    
    if (isLoading) {
        textAlign(LEFT, BOTTOM);
        text("Ładowaie utworu muzycznego" + dots[loadingNo], width / 4, (int)(0.68 * height));
        loadingNo = (loadingNo + 1) % 4;
        delay(100);
    }
    
    playButton.draw();
    rateX05Button.draw();
    rateX1Button.draw();
    rateX2Button.draw();
    openButton.draw();
    
    progressBar.draw();
    
    if (soundfile != null) {
        if (soundfile.isPlaying()) {
            fft.analyze(spectrum);
        }
        
        drawSpectrum();
    }
    
    textFont(font2);
    
    for (int i = 0; i < inputFields.size(); i++) {
        inputFields.get(i).draw();
    }
    
    dButton.draw();
    hpButton.draw();
    lpButton.draw();
    rButton.draw();
    //drawDelay();
    
    textAlign(LEFT, TOP);
    pushMatrix();
    translate(dPos.x, dPos.y);
    rotate(1.57);
    text("Delay", 0, 0);
    popMatrix();
    
    pushMatrix();
    translate(hpPos.x, hpPos.y);
    rotate(1.57);
    text("High Pass", 0, 0);
    popMatrix();
    
    pushMatrix();
    translate(lpPos.x, lpPos.y);
    rotate(1.57);
    text("Low Pass", 0, 0);
    popMatrix();
    
    pushMatrix();
    translate(rPos.x, rPos.y);
    rotate(1.57);
    text("Reverb", 0, 0);
    popMatrix();
    
    
    textFont(font);
    
    delay(100);
}

void drawSpectrum() {
    int xOffset = width / 2 / bands;
    int x = width / 4 + (int)(0.05 * xOffset);
    
    int yOffset = (int)(0.6 * height / colors.length);
    int y = (int)(0.68 * height) - yOffset;
    
    for (int i = 0; i < bands; i++) {
        int v = (int)(spectrum[i] * 1_000_000);
        
        for(int j = 0; j < treshholds.length; j++) {
            if (v > treshholds[j]) {
                fill(colors[j]);
                rect(x, y - j * yOffset, (int)(0.9 * xOffset), (int)(0.9 * yOffset));
            }
            else {
                break;
            }
        }
        x += xOffset; 
    }
}


void keyPressed() {
  if (key == 'o') {
    File file = new File(sketchPath("") + "/*.mp3");
    selectInput("Wybierz plik z :", "musicFileSelected", file);
  }
  
  if (selectedIF >= 0) {
      if ((key >= '0' && key <= '9') || key == '.') {
          inputFields.get(selectedIF).addDigit(key);
      }
      
      if (key == '\b') {
          inputFields.get(selectedIF).removeDigit();
      }
      
      UpdateIFVariable();
  }
}

void UpdateIFVariable() {
    switch (selectedIF) {
        case 0:
            delayTime = inputFields.get(0).getFloat();
            break;
            
        case 1:
            hpFreq = inputFields.get(1).getFloat();
            break;
            
        case 2:
            lpFreq = inputFields.get(2).getFloat();
            break;
            
        case 3:
            damp = inputFields.get(3).getFloat();
            break;
            
        case 4:
            room = inputFields.get(4).getFloat();
            break;
            
        case 5:
            wet = inputFields.get(5).getFloat();
            break;
        
        default:
            break;
    }
}

void mousePressed() {
    if (playButton.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            if (soundfile.isPlaying()) {
                pause();
            }
            else {
                play();
            }
        }
    }
    else if (rateX05Button.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            rateX05Button.setShape(rateRectR);
            rateX1Button.setShape(rateRect);
            rateX2Button.setShape(rateRect);
            soundfile.rate(0.5);
            rate = 0.5f;
        }
    }
    else if (rateX1Button.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            rateX05Button.setShape(rateRect);
            rateX1Button.setShape(rateRectR);
            rateX2Button.setShape(rateRect);
            soundfile.rate(1);
            rate = 1.0f;
        }
    }
    else if (rateX2Button.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            rateX05Button.setShape(rateRect);
            rateX1Button.setShape(rateRect);
            rateX2Button.setShape(rateRectR);
            soundfile.rate(2);
            rate = 2.0f;
        }
    }
    else if (openButton.contains(mouseX, mouseY)) {
        File file = new File(sketchPath("") + "/*.mp3");
        selectInput("Wybierz plik z :", "musicFileSelected", file);
    }
    else if (dButton.contains(mouseX, mouseY) && soundfile != null) {
      if (!dOn) {
          delay.process(soundfile, 5);
          delay.time(delayTime);
          dButton.setText("Włączony");
          dOn = true;
      }
      else {
          delay.stop();
          dButton.setText("Wyłączony");
          dOn = false;
      }
    }
    else if (hpButton.contains(mouseX, mouseY) && soundfile != null) {
      if (!hpOn) {
          highPass.process(soundfile, hpFreq);
          hpButton.setText("Włączony");
          hpOn = true;
      }
      else {
          highPass.stop();
          hpButton.setText("Wyłączony");
          hpOn = false;
      }
    }
    else if (lpButton.contains(mouseX, mouseY) && soundfile != null) {
      if (!lpOn) {
          lowPass.process(soundfile, lpFreq);
          lpButton.setText("Włączony");
          lpOn = true;
      }
      else {
          lowPass.stop();
          lpButton.setText("Wyłączony");
          lpOn = false;
      }
    }
    else if (rButton.contains(mouseX, mouseY) && soundfile != null) {
      if (!rOn) {
          reverb.set(room, damp, wet);
          reverb.process(soundfile);
          rButton.setText("Włączony");
          rOn = true;
      }
      else {
          reverb.stop();
          rButton.setText("Wyłączony");
          rOn = false;
      }
    }
    else {
        selectedIF = -1;
        for (int i = 0; i < inputFields.size(); i++) {
            if (inputFields.get(i).isClicked(mouseX, mouseY)) {
                selectedIF = i;
                break;
            }
        }
    }
}

void mouseDragged() {
    if (progressBar.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            boolean playing = soundfile.isPlaying();
            timeElapsed = progressBar.drag(mouseX, mouseY);
            soundfile.jump((float)(timeElapsed) / 1000.0f);
            
            if (!playing) {
               soundfile.pause(); 
            }
            
            int el = timeElapsed / 1000;
            elapsedText = "" + el / 3600 + ":" + (el / 60) % 60 + ":" + el % 60;
        }
    }
}


void pause() {
    if (soundfile != null) {
        playButton.setShape(play);
        soundfile.pause();
    }
}

void play() {
    if (soundfile != null) {
        playButton.setShape(pause);
        soundfile.play();
        
        lastTime = millis();
    }
}


void musicFileSelected(File selection) {
    String file = selection.getAbsolutePath();
    fileTitle = file.substring(file.lastIndexOf("\\") + 1, file.lastIndexOf("."));
    
    if (soundfile == null) {
        //soundfile = new SoundFile(this, file);
        openFile(file);
        return;
    }
    else if (soundfile.isPlaying()) {
        pause();
    }
    soundfile.removeFromCache();
    openFile(file);
}

void openFile(String filePath) {
    isLoading = true;
    soundfile = new SoundFile(this, filePath, false);
    isLoading = false;
    
    duration = (int)(soundfile.duration() * 1000);
    int dur = duration / 1000;
    durationText = "" + dur / 3600 + ":" + (dur / 60) % 60 + ":" + dur % 60;
    elapsedText = "0:0:0";
    
    progressBar.setValue(0);
    progressBar.setTotalValue(duration);
    
    fft.input(soundfile);
}
