import collections

import cv2
import time
#import numpy as np
import HandTrackingModule as htm
import math

#from ctypes import cast, POINTER
#from comtypes import CLSCTX_ALL
#from pycaw.pycaw import AudioUtilities, IAudioEndpointVolume


def trackDartPosition(radius, numberOfLastPos, cap=cv2.VideoCapture, returnTime=False): #function responsible for tracking position of dart and returning last of them and optionaly time of their occurence (ns); the higher index of position is, the more recent data are
    lastDartPositions = collections.deque(maxlen=numberOfLastPos) #deque object containing set of last "dart" (precisely fingers tip) positions
    timeOfPosOccurence = collections.deque(maxlen=numberOfLastPos) #temporary deque object containing time o last dart position
    detector = htm.HandDetector()

    while True:
        success, img = cap.read()
        img = detector.findHands(img)
        lmList = detector.getLandmarksOfHand(img, draw=False, landmarks=[4,8])  #[THUMB_TIP[id,x,y,z], INDEX_FINGER_TIP[id,x,y,z]]

        if lmList:
            x1, y1 = lmList[0][1], lmList[0][2] #thumb tip position
            x2, y2 = lmList[1][1], lmList[1][2] #index finger tip position
            if abs( (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) ) < radius*radius:
                timeOfPosOccurence.append(time.time_ns())
                lastDartPositions.append([(x1+x2)/2,(y1+y2)/2,(lmList[0][3] + lmList[0][3])/2])
                break

    while True:
        success, img = cap.read()
        img = detector.findHands(img)
        lmList = detector.getLandmarksOfHand(img, draw=False, landmarks=[4, 8])  # [THUMB_TIP[id,x,y,z], INDEX_FINGER_TIP[id,x,y,z]]

        if lmList:
            x1, y1 = lmList[0][1], lmList[0][2]  # thumb tip position
            x2, y2 = lmList[1][1], lmList[1][2]  # index finger tip position
            if abs((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) > radius * radius:
                break
            timeOfPosOccurence.append(time.time_ns())
            lastDartPositions.append([(x1 + x2) / 2, (y1 + y2) / 2, (lmList[0][3] + lmList[1][3]) / 2])

    if returnTime:
        return lastDartPositions, timeOfPosOccurence
    return lastDartPositions

def calculateVelocity(timeDif, pos1 = [], pos2 = []): #this function calculates the velocity (basing on the movement between two positions and the time it was achived (expresed in ns) ), returning 3d vector of its expressed in (pos. unit / s)
    return [(pos2[0] - pos1[0])/(timeDif*0.000000001),(pos2[1] - pos1[1])/(timeDif*0.000000001),(pos2[2] - pos1[2])/(timeDif*0.000000001)]



def main():
    #settings of widow displaying catched view of camera
    wCam, hCam = 640, 380
    cap = cv2.VideoCapture(0)
    cap.set(3, wCam)
    cap.set(4, hCam)

    #pTime = 0

    cRadius = 0.1*wCam #variable responsible for expressing the distance in pixels below which the fingertips touch
    lastDartPositions, timeOfPosOccurence = trackDartPosition(cRadius, 2, cap, returnTime=True)
    velocityVector = calculateVelocity(timeOfPosOccurence[1]-timeOfPosOccurence[0],lastDartPositions[1],lastDartPositions[0]) #velocity in x, y, z axis



    """cTime = time.time()
    fps = int(1 / (cTime - pTime))
    pTime = cTime
    print("FPS: ", fps)
    cv2.putText(img, f"FPS: {fps}", (20, 30), cv2.FONT_HERSHEY_PLAIN, 1, (255, 0, 0), 3)

    cv2.imshow("Image", img)
    cv2.waitKey(1)"""


if __name__ == "__main__":
    main()
