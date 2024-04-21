import math

import cv2
import mediapipe as mp
import time


class HandDetector():
    def __init__(self, mode=False, maxHands = 2, detectionCon=0.5, trackCon=0.5):
        self.mode = mode
        self.maxHands = maxHands
        self.detectionCon = detectionCon
        self.trackCon = trackCon

        self.mpHands = mp.solutions.hands
        self.hands = self.mpHands.Hands(self.mode, self.maxHands, 1, self.detectionCon, self.trackCon)
        self.mpDraw = mp.solutions.drawing_utils

    def findHands(self, img, draw = False): #is resposible for processing the image by for detecting hands and their landmarks (it eventually marks them)
        imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        self.results = self.hands.process(imgRGB)
        # print(results.multi_hand_landmarks)

        if self.results.multi_hand_landmarks:
            for handLms in self.results.multi_hand_landmarks:
                if draw:
                    self.mpDraw.draw_landmarks(img, handLms, self.mpHands.HAND_CONNECTIONS)
        return img

    def getLandmarksOfHand(self, img, handNo = 0, draw = False, landmarks = []): #find all landmarks (or of given numbers) position for hand of given number (default hand number is 0), eventually this function marks them
        lmList = []

        """i = 0
        if self.results.multi_hand_landmarks:
            for h in self.results.multi_hand_landmarks:
                i += 1"""

        if self.results.multi_hand_landmarks and handNo < len(self.results.multi_hand_landmarks):
            myHand = self.results.multi_hand_landmarks[handNo]

            h, w, c = img.shape

            if landmarks:
                for id in landmarks:
                    # print(id, lm)
                    lm = myHand.landmark[id][1]
                    #h, w, c = img.shape
                    cx, cy = int(lm.x * w), int(lm.y * h)
                    #print(id, cx, cy)
                    lmList.append([id, cx, cy, lm.z])
                    if draw:
                        cv2.circle(img, (cx, cy), 10, (255, 0, 255), cv2.FILLED)
            else:
                for id, lm in enumerate(myHand.landmark):
                    cx, cy = int(lm.x * w), int(lm.y * h)
                    lmList.append([id, cx, cy, lm.z])
                    if draw:
                        cv2.circle(img, (cx, cy), 10, (255, 0, 255), cv2.FILLED)


        return lmList

def calibrationTest():
    wCam, hCam = 640, 380
    cap = cv2.VideoCapture(0)
    cap.set(3, wCam)
    cap.set(4, hCam)

    file = open("DataTOAnalyse.txt","w")
    file.write(f"w: {wCam}; h: {hCam}")

    detector = HandDetector()
    cRadius = 10

    while True:
        success, img = cap.read()

        img = detector.findHands(img)

        lmList0 = detector.getLandmarksOfHand(img)
        lmList1 = detector.getLandmarksOfHand(img, handNo=1)

        if lmList0 and lmList1:
            if (abs((lmList0[4][1] - lmList0[8][1]) * (lmList0[4][1] - lmList0[8][1]) + (lmList0[4][2] - lmList0[8][2]) * (lmList0[4][2] - lmList0[8][2])) > cRadius * cRadius) and (abs((lmList1[4][1] - lmList1[8][1]) * (lmList1[4][1] - lmList1[8][1]) + (lmList1[4][2] - lmList1[8][2]) * (lmList1[4][2] - lmList1[8][2])) > cRadius * cRadius):
                p1 = [(lmList0[4][1] + lmList0[8][1])/2,(lmList0[4][2] + lmList0[8][2])/2,(lmList0[4][3] + lmList0[8][3])/2]
                p2 = [(lmList1[4][1] + lmList1[8][1])/2,(lmList1[4][2] + lmList1[8][2])/2,(lmList1[4][3] + lmList1[8][3])/2]
                file.write(f"\n\n\n##########################################\n\n\nh0: ( {p1[0]} , {p1[1]} , {p1[2]} ); \n\nh1: ( {p2[0]} , {p2[1]} , {p2[2]} )\n\ndistance = {math.sqrt((p1[0]-p2[0])*(p1[0]-p2[0])+(p1[1]-p2[1])*(p1[1]-p2[1])+(p1[2]-p2[2])*(p1[2]-p2[2]))}")

        #cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_SIMPLEX, 3, (255, 0, 255), 3)

        cv2.imshow("Image", img)
        cv2.waitKey(1)

def main():
    calibrationTest()
    """pTime = 0
    cTime = 0
    cap = cv2.VideoCapture(0)
    detector = HandDetector()

    while True:
        success, img = cap.read()

        img = detector.findHands(img)
        lmList = detector.getLandmarksOfHand(img)
        if len(lmList) != 0:
            print(lmList[0])

        cTime = time.time()
        fps = 1 / (cTime - pTime)
        pTime = cTime
        cv2.putText(img, str(int(fps)), (10, 70), cv2.FONT_HERSHEY_SIMPLEX, 3, (255, 0, 255), 3)

        cv2.imshow("Image", img)
        cv2.waitKey(1)"""

if __name__ == "__main__":
    main()