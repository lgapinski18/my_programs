#pragma once

#ifndef PLANEPLAYERCONTROLLER_H
#define PLANEPLAYERCONTROLLER_H

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include "GameObject.h";

class PlanePLayerController {
	private:
		GameObject* plane;

        float maxVelocity = 1000.0f;
        float velocity = 0.0f;
        float propulsionAccel = 100.0f;
        float resistanceAccel = 20.0f;
        float g = 98.0f;
        glm::vec3 V = glm::vec3(0, 0, 0);

        float rotationVelocity = 0.1;
        float Roll = 0.0f;
        float Yawn = 0.0f;
        float Pitch = 0.0f;

        glm::vec3 Direction = glm::vec3(1.0f, 0.0f, 0.0f);
        glm::vec3 Right = glm::vec3(0.0f, 0.0f, 1.0f);

        glm::vec3 getRotAngleFromDir(glm::vec3 direction) {
            float xAngle = 0;
            float yAngle = 0;
            float zAngle = 0;

            if (direction.x == -1) {
                yAngle = 3.14;
            }

            if ((direction.x != 1 && direction.x != -1)
                || direction.y != 0 || direction.z != 0) {
                glm::vec3 rotN = glm::cross(glm::vec3(1, 0, 0), direction);
                float rotAngle = acos(glm::dot(glm::vec3(1, 0, 0), direction));

                glm::mat4 rotM = glm::mat4(1.0f);
                rotM = glm::rotate(rotM, rotAngle, rotN);

                float R31 = rotM[0][2];


                if (R31 < 1 && R31 > -1) {
                    float R21 = rotM[0][1];
                    float R11 = rotM[0][0];
                    float R32 = rotM[1][2];
                    float R33 = rotM[2][2];

                    yAngle = -asin(R31);
                    xAngle = atan2(R32, R33);
                    zAngle = atan2(R21, R11);
                }
                else {
                    zAngle = 0;

                    float R12 = rotM[1][0];
                    float R13 = rotM[2][0];

                    if (R31 == -1) {
                        yAngle = 1.57f;
                        xAngle = zAngle + atan2(R12, R13);
                    }
                    else {
                        yAngle = -1.57f;
                        xAngle = -zAngle + atan2(-R12, -R13);
                    }
                }
            }

            return glm::vec3(xAngle, yAngle, zAngle);
        }
        		
	public:
        enum PlaneMovement {
            ROT_UP,
            ROT_DOWN,
            ROT_LEFT,
            ROT_RIGHT,
            ROLL_LEFT,
            ROLL_RIGHT,
            PROPEL
        };

		PlanePLayerController(GameObject* plane) : plane(plane) {
		}

        void Update(float deltaTime) {
            //float distance = velocity * deltaTime;
            //plane->getTransform()->setTranslate(plane->getTransform()->getTranslate() + Direction * distance);
            plane->getTransform()->setTranslate(plane->getTransform()->getTranslate() + V * deltaTime);

            if (velocity > 0) {
                float v = velocity;
                velocity -= resistanceAccel * deltaTime;

                if (velocity > 0) {
                    V -= Direction * resistanceAccel * deltaTime;
                }
                else {
                    V -= Direction * v;
                    velocity = 0;
                }
            }

            V += glm::vec3(0, -1, 0) * g * deltaTime;

            if (plane->getTransform()->getTranslate().y < 0) {
                V.y = 0;

                glm::vec3 pos = plane->getTransform()->getTranslate();
                pos.y = 0;
                plane->getTransform()->setTranslate(pos);

                glm::vec3 rot = getRotAngleFromDir(Direction);
                rot.x = 0;
                rot.z = 0;
                plane->getTransform()->setRotation(rot);
            }
        }

        glm::vec3 getUp() {
            return glm::normalize(glm::cross(Right, Direction));
        }

        void ProcessKeyboard(PlaneMovement movement, float deltaTime)
        {
            switch (movement) {
                case ROT_UP:
                    Pitch += rotationVelocity * deltaTime;
                    break;

                case ROT_DOWN:
                    Pitch -= rotationVelocity * deltaTime;
                    break;

                case ROT_LEFT:
                    Yawn += rotationVelocity * deltaTime;
                    break;

                case ROT_RIGHT:
                    Yawn -= rotationVelocity * deltaTime;
                    break;

                case ROLL_LEFT:
                    Roll -= 0.5 * rotationVelocity * deltaTime;
                    break;

                case ROLL_RIGHT:
                    Roll += 0.5 * rotationVelocity * deltaTime;
                    break;

                case PROPEL:
                    velocity += propulsionAccel * deltaTime;
                    if (velocity > maxVelocity) {
                        velocity = maxVelocity;
                    }

                    V = Direction * velocity;

                    break;

            }
        }

        void UpdateVectors() {
            glm::mat4 unit = glm::mat4(1.0f);

            Direction = glm::vec3(1, 0, 0);
            Right = glm::vec3(0, 0, 1);

            glm::mat4 yawnRot = glm::rotate(unit, Yawn, glm::vec3(0, 1, 0));
            Direction = glm::normalize(glm::vec3(yawnRot * glm::vec4(Direction, 1.0)));
            Right = glm::normalize(glm::vec3(yawnRot * glm::vec4(Right, 1.0)));

            glm::mat4 rollRot = glm::rotate(unit, Roll, Direction);
            Right = glm::normalize(glm::vec3(rollRot * glm::vec4(Right, 1.0)));

            glm::mat4 pitchRot = glm::rotate(unit, Pitch, Right);
            Direction = glm::normalize(glm::vec3(pitchRot * glm::vec4(Direction, 1.0)));

            //std::cout << "\n" << Roll << "\t" << Yawn << "\t" << Pitch;
            //std::cout << "\n" << Direction.x << "\t" << Direction.y << "\t" << Direction.z << "\t" << Right.x << "\t" << Right.y << "\t" << Right.z;;

            //plane->getTransform()->setRotation(glm::vec3(Roll, Yawn, Pitch));

            pitchRot *= rollRot;
            pitchRot *= yawnRot;
            plane->getTransform()->setRotationMatrix(pitchRot);
        }
};

#endif