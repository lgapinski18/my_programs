import cv2
import time
import numpy as np
import HandTrackingModule as htm
import math

from ctypes import cast, POINTER
from comtypes import CLSCTX_ALL
from pycaw.pycaw import AudioUtilities, IAudioEndpointVolume


wCam, hCam = 640, 380

cap = cv2.VideoCapture(0)
cap.set(3, wCam)
cap.set(4, hCam)

detector = htm.handDetector()

### inicjalizacja
devices = AudioUtilities.GetSpeakers()
interface = devices.Activate(
    IAudioEndpointVolume._iid_, CLSCTX_ALL, None)
volume = cast(interface, POINTER(IAudioEndpointVolume))
###
volRange = volume.GetVolumeRange()
minVol = volRange[0]
maxVol = volRange[1]

print(maxVol, minVol)

pTime = 0

minVL = 50
maxVL = 250

volSetPer = int(np.interp(volume.GetMasterVolumeLevel(), [minVol, maxVol], [0, 100]))
#volPerSet = int((volume.GetMasterVolumeLevel() - minVol)/(maxVol - minVol) * 100)
volCurPer = volSetPer

while True:
    success, img = cap.read()

    img = detector.findHands(img)
    lmList = detector.findPosition(img, draw=False)

    if len(lmList) != 0:
        #print(lmList[4], lmList[8])

        x1, y1 = lmList[4][1], lmList[4][2]
        x2, y2 = lmList[8][1], lmList[8][2]
        x3, y3 = lmList[12][1], lmList[12][2]
        cx, cy = int((x1 + x2)/2), int((y1 + y2)/2)

        cv2.circle(img, (x1, y1), 15, (255, 0, 255), cv2.FILLED)
        cv2.circle(img, (x2, y2), 15, (255, 0, 255), cv2.FILLED)
        cv2.line(img, (x1, y1), (x2, y2), (255, 0, 255), 3)
        cv2.circle(img, (cx, cy), 15, (255, 0, 255), cv2.FILLED)

        length = math.hypot(x2 - x1, y2 - y1)
        #print(length)
        #vol = (length - minVL)/(maxVL - minVL) * (maxVol - minVol) + minVol
        if (x3 - x2)*(x3 - x2) + (y3 - y2)*(y3 - y2) <= 40*40:
            vol = np.interp(length, [minVL, maxVL], [minVol, maxVol])
            print(int(length), vol)
            volume.SetMasterVolumeLevel(vol, None)
            volSetPer = int(np.interp(length, [minVL, maxVL], [0, 100]))

        volCurPer = int(np.interp(length, [minVL, maxVL], [0, 100]))

        if length <= minVL:
            cv2.circle(img, (cx, cy), 15, (0, 255, 0), cv2.FILLED)
        elif length >= maxVL:
            cv2.circle(img, (cx, cy), 15, (0, 0, 255), cv2.FILLED)
        else:
            difVL = maxVL - minVL
            r, g = int((length - minVL)/difVL*255), int((maxVL - length)/difVL*255)
            cv2.circle(img, (cx, cy), 15, (0, g, r), cv2.FILLED)

        cv2.circle(img, (x3, y3), 40, (255, 0, 0), 3)

    cv2.putText(img, f"Set: {volSetPer}%", (20, 60), cv2.FONT_HERSHEY_PLAIN, 1, (255, 0, 0), 3)
    cv2.putText(img, f"Cur: {volCurPer}%", (20, 80), cv2.FONT_HERSHEY_PLAIN, 1, (255, 0, 0), 3)

    cTime = time.time()
    fps = int(1/(cTime - pTime))
    pTime = cTime
    cv2.putText(img, f"FPS: {fps}", (20, 30), cv2.FONT_HERSHEY_PLAIN, 1, (255, 0, 0), 3)

    cv2.imshow("Image", img)
    cv2.waitKey(1)

