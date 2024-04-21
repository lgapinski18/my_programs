#pragma once

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>


struct DirectionalLightData {
    bool DirectionalLightOn;
    glm::vec4 DirectionalLightColor;
    glm::vec3 DirectionalLightDir;
    float DirectionalLightPower;
};

class DirectionalLight : public GameObject {
private:
    DirectionalLightData* directionalLightData;

public:
    DirectionalLight(Model* model3d, Shader* shader, Transform* transform, DirectionalLightData* directionalLightData)
        : GameObject(model3d, shader, transform), directionalLightData(directionalLightData) {
    }

    DirectionalLightData* getLightData() {
        return directionalLightData;
    }

    void Draw(glm::mat4& model) override {

        if (!directionalLightData->DirectionalLightOn) {
            return;
        }

        getShader()->use();
        getShader()->setVec4("inColor",
            glm::vec4(directionalLightData->DirectionalLightColor.x, directionalLightData->DirectionalLightColor.y, directionalLightData->DirectionalLightColor.z, 1));

        GameObject::Draw(model);
    }

    void UpdateLightData(Shader* shader1, Shader* shader2) {
        shader1->use();

        shader1->setBool("DirectionalLightOn", directionalLightData->DirectionalLightOn);
        shader1->setVec4("DirectionalLightColor", directionalLightData->DirectionalLightColor);
        glm::vec3 normalized = glm::normalize(directionalLightData->DirectionalLightDir);
        directionalLightData->DirectionalLightDir.x = normalized.x;
        directionalLightData->DirectionalLightDir.y = normalized.y;
        directionalLightData->DirectionalLightDir.z = normalized.z;
        shader1->setVec3("DirectionalLightDir", directionalLightData->DirectionalLightDir);
        shader1->setFloat("DirectionalLightPower", directionalLightData->DirectionalLightPower);

        shader2->use();

        shader2->setBool("DirectionalLightOn", directionalLightData->DirectionalLightOn);
        shader2->setVec4("DirectionalLightColor", directionalLightData->DirectionalLightColor);
        shader2->setVec3("DirectionalLightDir", directionalLightData->DirectionalLightDir);
        shader2->setFloat("DirectionalLightPower", directionalLightData->DirectionalLightPower);


        float xAngle = 0;
        float yAngle = 0;
        float zAngle = 0;

        if (directionalLightData->DirectionalLightDir.x == -1) {
            yAngle = 3.14;
        }

        if ((directionalLightData->DirectionalLightDir.x != 1 && directionalLightData->DirectionalLightDir.x != -1) 
            || directionalLightData->DirectionalLightDir.y != 0 || directionalLightData->DirectionalLightDir.z != 0) {
            glm::vec3 rotN = glm::cross(glm::vec3(1, 0, 0), directionalLightData->DirectionalLightDir);
            float rotAngle = acos(glm::dot(glm::vec3(1, 0, 0), directionalLightData->DirectionalLightDir));

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

        getTransform()->setRotation(glm::vec3(xAngle, yAngle, zAngle));
    }
};