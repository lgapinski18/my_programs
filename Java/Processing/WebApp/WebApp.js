//import * as psound from "p5.sound.min";
//import "libraries/p5.sound.min.js"; //https://p5js.org/reference/#/libraries/p5.sound

var sine = new p5.Oscillator('sine');
var tri = new p5.TriOsc();
var sqr = new p5.SqrOsc();
var saw = new p5.SawOsc();

var SinPlaying = false;
var TriPlaying = false;
var SqrPlaying = false;
var SawPlaying = false;


function setup() {
    createCanvas(width, height);
    
    sine.freq(100);
    sine.amp(1);
    sine.pan(0.0);
    
    tri.freq(100);
    tri.amp(1);
    tri.pan(0.0);
    
    sqr.freq(100);
    sqr.amp(1);
    sqr.pan(0.0);
    
    saw.freq(100);
    saw.amp(1);
    saw.pan(0.0);
    
    /*const value = document.querySelector("#SinFregOut");
    const input = document.querySelector("#SinFregNum");
    value.textContent = input.value;
    input.addEventListener("input", (event) => {
      value.textContent = event.target.value;
    });*/
}


/*function draw() {
    background(220);
}*/

function AmpSin() {
  var amp = document.querySelector("#SinAmpNum");
  sine.amp(parseFloat(amp.value));
}

function AmpTri() {
  var amp = document.querySelector("#TriAmpNum");
  tri.amp(parseFloat(amp.value));
}

function AmpSqr() {
  var amp = document.querySelector("#SqrAmpNum");
  sqr.amp(parseFloat(amp.value));
}

function AmpSaw() {
  var amp = document.querySelector("#SawAmpNum");
  saw.amp(parseFloat(amp.value));
}


function FreqSin() {
  var freq = document.querySelector("#SinFregNum");
  sine.freq(parseInt(freq.value));
}

function FreqTri() {
  var freq = document.querySelector("#TriFregNum");
  tri.freq(parseInt(freq.value));
}

function FreqSqr() {
  var freq = document.querySelector("#SqrFregNum");
  sqr.freq(parseInt(freq.value));
}

function FreqSaw() {
  var freq = document.querySelector("#SawFregNum");
  saw.freq(parseInt(freq.value));
}


function PanSin() {
  var pan = document.querySelector("#SinPanSlideBar");
  sine.pan(parseFloat(pan.value));
}

function PanTri() {
  var pan = document.querySelector("#TriPanSlideBar");
  tri.pan(parseFloat(pan.value));
}

function PanSqr() {
  var pan = document.querySelector("#SqrPanSlideBar");
  sqr.pan(parseFloat(pan.value));
}

function PanSaw() {
  var pan = document.querySelector("#SawPanSlideBar");
  saw.pan(parseFloat(pan.value));
}


function PlayPauseSin() {
  var playButton = document.querySelector("#SinPlayButton");
  
  if (SinPlaying) {
    sine.stop();
    SinPlaying = false;
	  playButton.setAttribute('class', 'playButton');
  }
  else {
    sine.start();
    SinPlaying = true;
    playButton.setAttribute('class', 'pauseButton');
  }
}

function PlayPauseTri() {
  var playButton = document.querySelector("#TriPlayButton");
  
  if (TriPlaying) {
    tri.stop();
    TriPlaying = false;
    playButton.setAttribute('class', 'playButton');
  }
  else {
    tri.start();
    TriPlaying = true;
    playButton.setAttribute('class', 'pauseButton');
  }
}

function PlayPauseSqr() {
  var playButton = document.querySelector("#SqrPlayButton");
  
  if (SqrPlaying) {
    sqr.stop();
    SqrPlaying = false;
    playButton.setAttribute('class', 'playButton');
  }
  else {
    sqr.start();
    SqrPlaying = true;
    playButton.setAttribute('class', 'pauseButton');
  }
}

function PlayPauseSaw() {
  var playButton = document.querySelector("#SawPlayButton");
  
  if (SawPlaying) {
    saw.stop();
    SawPlaying = false;
    playButton.setAttribute('class', 'playButton');
  }
  else {
    saw.start();
    SawPlaying = true;
    playButton.setAttribute('class', 'pauseButton');
  }
}
