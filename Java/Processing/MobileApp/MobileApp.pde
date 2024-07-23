import processing.sound.*;
import android.content.Intent;
import android.net.Uri;
import android.app.Activity;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.nio.file.Path;

SoundFile soundfile;
String fileTitle = "Niewybrano utworu";

String fileDest = "/data/user/0/processing.test.mobileapp/files/";
    
PFont font;


float CX = 0;
float CY = 0;
float R = 0;

MButton playButton;
MButton rateX05Button;
MButton rateX1Button;
MButton rateX2Button;
MButton openButton;

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
AudioIn audioIn;
int bands = 16;
float[] spectrum = new float[bands];
color[] colors = {color(0, 255, 0), color(43, 212, 0), color(86, 169, 0), color(129, 129, 0), color(169, 86, 0), color(212, 43, 0), color(255, 0, 0)};
int[] treshholds = {0, 10, 30, 50, 100, 200, 400};


void setup() {
    fullScreen();
    requestPermission("android.permission.READ_EXTERNAL_STORAGE", "checkPermissionGranted");
    requestPermission("android.permission.RECORD_AUDIO", "checkPermissionGranted2");
    
    
    fft = new FFT(this, bands);
    audioIn = new AudioIn(this, 0);
    audioIn.play();
    fft.input(audioIn);
    
    font = createFont("Calibri", 25);
    textFont(font);
    
    
    CX = width / 2;
    CY = 0.85 * height;
    R = 0.1 * height;
    
    
    playButton = new CircleButton(CX, CY, R);
    
    float w = 0.1 * width;
    float h = 0.05 * height;
    
    strokeWeight(2);
    rateX05Button = new RectButton(0.1 * width, 0.9 * height, w, h, "0.5");
    rateX1Button = new RectButton(0.2 * width + 10, 0.9 * height, w, h, "1");
    rateX2Button = new RectButton(0.3 * width + 20, 0.9 * height, w, h, "2");
    openButton = new RectButton(0.825 * width, 0.9 * height, 0.25 * width, 0.05 * height, "otworz mp3");

    progressBar = new ProgressBar((int)(0.05 * width), (int)(0.7 * height), (int)(0.95 * width), (int)(0.7 * height), 10);
}

void checkPermissionGranted(boolean granted) {
  if (granted) {   
    println("ES available");
    // ...
  } else {
    println("ES is not available");
    // ...
  }
}

void checkPermissionGranted2(boolean granted) {
  if (granted) {   
    println("RA available");
    // ...
  } else {
    println("RA is not available");
    // ...
  }
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
    text(fileTitle, 0.05 * width, 0.05 * height);
    
    text(elapsedText, 0.05 * width, 0.8 * height);
    textAlign(RIGHT, TOP);
    text(durationText, 0.95 * width, 0.8 * height);
    
    
    if (isLoading) {
        textAlign(LEFT, BOTTOM);
        text("Ładowaie utworu muzycznego" + dots[loadingNo], width / 4, (int)(0.68 * height));
        loadingNo = (loadingNo + 1) % 4;
        delay(100);
    }
    
    //fill(150);
    //circle(CX, CY, R);
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
    
    delay(100);
}

void drawSpectrum() {
    int xOffset = width / 2 / bands;
    int x = width / 4 + (int)(0.05 * xOffset);
    
    int yOffset = (int)(0.6 * height / colors.length);
    int y = (int)(0.68 * height) - yOffset;
    
    for (int i = 0; i < bands; i++) {
        //System.out.println(i + ".  " + (int)(spectrum[i] * 1_000_000));
        int v = (int)(spectrum[i] * 1_000_000);
        
        //treshholds
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
            rateX05Button.setSelected(true);
            rateX1Button.setSelected(false);
            rateX2Button.setSelected(false);
            soundfile.rate(0.5);
            rate = 0.5f;
        }
    }
    else if (rateX1Button.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            rateX05Button.setSelected(false);
            rateX1Button.setSelected(true);
            rateX2Button.setSelected(false);
            soundfile.rate(1);
            rate = 1.0f;
        }
    }
    else if (rateX2Button.contains(mouseX, mouseY)) {
        if (soundfile != null) {
            rateX05Button.setSelected(false);
            rateX1Button.setSelected(false);
            rateX2Button.setSelected(true);
            soundfile.rate(2);
            rate = 2.0f;
        }
    }
    else if (openButton.contains(mouseX, mouseY)) {
        Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
        intent.setType("audio/mpeg");
        getActivity().startActivityForResult(intent, SELECT_FILE_REQUEST_CODE);
    }
}

static final int SELECT_FILE_REQUEST_CODE = 1;
void onActivityResult(int requestCode, int resultCode, Intent data) {
    if (requestCode == SELECT_FILE_REQUEST_CODE) {
        if (resultCode == Activity.RESULT_OK) {
            if (requestCode == 1) {
                Uri selectedFileUri = data.getData();
                String filePath = selectedFileUri.getPath();//getPath(selectedFileUri);
                filePath = "/storage/emulated/0/" + filePath.substring(filePath.lastIndexOf(":") + 1);
                println("Selected file path: " + filePath);
                musicFileSelected(new File(filePath));
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
        playButton.setSelected(false);
        soundfile.pause();
    }
}

void play() {
    if (soundfile != null) {
        playButton.setSelected(true);
        soundfile.play();
        
        lastTime = millis();
    }
}

void musicFileSelected(File selection) {
    String file = selection.getAbsolutePath();
    
    if (soundfile != null) {
        if (soundfile.isPlaying()) {
            pause();
        }
        soundfile.removeFromCache();
        
        File toDelete = new File(fileDest + fileTitle);
        toDelete.delete();        
    }
    
    fileTitle = file.substring(file.lastIndexOf("/") + 1);
    copyFile(selection);
    
    if (soundfile == null) {
        //soundfile = new SoundFile(this, file);
        openFile(fileTitle);
        return;
    }
    else if (soundfile.isPlaying()) {
        pause();
    }
    soundfile.removeFromCache();
    openFile(fileTitle);
}

void openFile(String filePath) {
    isLoading = true;
    soundfile = new SoundFile(this, filePath, false);
    isLoading = false;
    //System.out.println("Załądowano nowy plik");
    duration = (int)(soundfile.duration() * 1000);
    //duration = 60 * 1000;
    int dur = duration / 1000;
    durationText = "" + dur / 3600 + ":" + (dur / 60) % 60 + ":" + dur % 60;
    elapsedText = "0:0:0";
    
    progressBar.setValue(0);
    progressBar.setTotalValue(duration);
}

void copyFile(File original) {
    /**/
    println("...");
    File dest = new File(fileDest + fileTitle);
    
    try {
        dest.createNewFile();
        Files.copy(original.toPath(), dest.toPath(), StandardCopyOption.REPLACE_EXISTING, StandardCopyOption.COPY_ATTRIBUTES);
        dest.setExecutable(true, false);
        dest.setReadable(true, false);
        dest.setWritable(true, false);
    }
    catch(IOException io) {
        println("==================");
        io.printStackTrace();
        println("==================");
    }/**/
}

void exit() {
    super.exit();
    
    if (soundfile != null) {
        if (soundfile.isPlaying()) {
            pause();
        }
        soundfile.removeFromCache();
        
        File toDelete = new File(fileDest + fileTitle);
        toDelete.delete();        
    }
}
