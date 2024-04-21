#pragma once
#ifndef CAMERAOBJECT_H
#define CAMERAOBJECT_H

#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>

#include <vector>
#include "Transform.h"


// An abstract camera class that processes input and calculates the corresponding Euler Angles, Vectors and Matrices for use in OpenGL
class CameraObject : public GameObject
{
public:
    // camera Attributes
    //glm::vec3 Position;
    glm::vec3 Up;
    /*glm::vec3 Right;
    glm::vec3 Front;
    glm::vec3 WorldUp;
    // euler Angles
    float Yaw;
    float Pitch;
    // camera options
    float MovementSpeed;
    float MouseSensitivity;
    float Zoom;

    const float YAW = -90.0f;
    const float PITCH = 0.0f;
    const float SPEED = 50.0f;
    const float SENSITIVITY = 0.1f;
    const float ZOOM = 45.0f;*/

    // constructor with vectors
    //CameraObject(glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f), float yaw = YAW, float pitch = PITCH) : Front(glm::vec3(0.0f, 0.0f, -1.0f)), MovementSpeed(SPEED), MouseSensitivity(SENSITIVITY), Zoom(ZOOM)
    CameraObject(glm::vec3 position = glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3 up = glm::vec3(0.0f, 1.0f, 0.0f)) 
    {
        Transform* transform = new Transform();
        transform->setTranslate(position);
        //((GameObject*)this)->;
        setTransform(transform);
        
        Up = up;

        //Position = position;
        //WorldUp = up;
        //Yaw = yaw;
        //Pitch = pitch;
        //updateCameraVectors();
    }

    // returns the view matrix calculated using Euler Angles and the LookAt Matrix
    glm::mat4 GetViewMatrix()
    {
        glm::mat4 matrix = parent->getAbsoluteTransformationMatrix();
        return glm::lookAt(glm::vec3(matrix * glm::vec4(transform->getTranslate(), 1)), glm::vec3(matrix * glm::vec4(0, 0, 0, 1)), Up);
    }

//private:
    // calculates the front vector from the Camera's (updated) Euler Angles
   /* void updateCameraVectors()
    {
        // calculate the new Front vector
        glm::vec3 front;
        front.x = cos(glm::radians(Yaw)) * cos(glm::radians(Pitch));
        front.y = sin(glm::radians(Pitch));
        front.z = sin(glm::radians(Yaw)) * cos(glm::radians(Pitch));
        Front = glm::normalize(front);
        // also re-calculate the Right and Up vector
        Right = glm::normalize(glm::cross(Front, WorldUp));  // normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
        Up = glm::normalize(glm::cross(Right, Front));
    }*/
};
#endif