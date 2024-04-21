#pragma once

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <cmath>


struct SpotLightData {
    bool SpotLightOn;
    glm::vec4 SpotLightColor;
    glm::vec3 SpotLightPos;
    glm::vec3 SpotLightDir;
    float coneAngle;
    float concentration;
    float SpotLightRadius;
    float SpotLightPower;
};

class SpotLight : public GameObject {
    private:
        SpotLightData* spotLightData;
        std::string idLabel = 0;

    public:
        SpotLight(Model* model3d, Shader* shader, Transform* transform, SpotLightData* spotLightData, std::string idLabel)
                                : GameObject(model3d, shader, transform), spotLightData(spotLightData), idLabel(idLabel) {
        }

        SpotLightData* getLightData() {
            return spotLightData;
        }

        void Draw(glm::mat4& model) override {

            if (!spotLightData->SpotLightOn) {
                return;
            }

            glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            getShader()->use();
            glm::vec4 color = glm::vec4(spotLightData->SpotLightColor.x, spotLightData->SpotLightColor.y, spotLightData->SpotLightColor.z, 0.5);
            getShader()->setVec4("inColor", color);

            GameObject::Draw(model);

            glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
        }

        void UpdateLightData(Shader* shader1, Shader* shader2) {
            shader1->use();
            shader1->setBool("SpotLightOn" + idLabel, spotLightData->SpotLightOn);
            shader1->setVec4("SpotLightColor" + idLabel, spotLightData->SpotLightColor);
            shader1->setVec3("SpotLightPos" + idLabel, spotLightData->SpotLightPos);
            glm::vec3 normalized = glm::normalize(spotLightData->SpotLightDir);
            spotLightData->SpotLightDir.x = normalized.x;
            spotLightData->SpotLightDir.y = normalized.y;
            spotLightData->SpotLightDir.z = normalized.z;
            shader1->setVec3("SpotLightDir" + idLabel, spotLightData->SpotLightDir);
            shader1->setFloat("CosConeAngle" + idLabel, cos(spotLightData->coneAngle));
            shader1->setFloat("concentration" + idLabel, spotLightData->concentration);
            shader1->setFloat("SpotLightRadius" + idLabel, spotLightData->SpotLightRadius);
            shader1->setFloat("SpotLightPower" + idLabel, spotLightData->SpotLightPower);

            shader2->use();
            shader2->setBool("SpotLightOn" + idLabel, spotLightData->SpotLightOn);
            shader2->setVec4("SpotLightColor" + idLabel, spotLightData->SpotLightColor);
            shader2->setVec3("SpotLightPos" + idLabel, spotLightData->SpotLightPos);
            shader2->setVec3("SpotLightDir" + idLabel, spotLightData->SpotLightDir);
            shader2->setFloat("CosConeAngle" + idLabel, cos(spotLightData->coneAngle));
            shader2->setFloat("concentration" + idLabel, spotLightData->concentration);
            shader2->setFloat("SpotLightRadius" + idLabel, spotLightData->SpotLightRadius);
            shader2->setFloat("SpotLightPower" + idLabel, spotLightData->SpotLightPower);


            getTransform()->setTranslate(glm::vec3(spotLightData->SpotLightPos.x,
                                                   spotLightData->SpotLightPos.y, 
                                                   spotLightData->SpotLightPos.z));
            /*getTransform()->setTranslate(glm::vec3(spotLightData->SpotLightPos.x / getTransform()->getScale().x,
                spotLightData->SpotLightPos.y / getTransform()->getScale().y,
                spotLightData->SpotLightPos.z / getTransform()->getScale().z));/**/

            getTransform()->setScale(glm::vec3(spotLightData->SpotLightRadius, tan(spotLightData->coneAngle) * spotLightData->SpotLightRadius, tan(spotLightData->coneAngle) * spotLightData->SpotLightRadius));
            

            float xAngle = 0;
            float yAngle = 0;
            float zAngle = 0;

            if (spotLightData->SpotLightDir.x == -1) {
                yAngle = 3.14;
            }

            if ((spotLightData->SpotLightDir.x != 1 && spotLightData->SpotLightDir.x != -1) 
                || spotLightData->SpotLightDir.y != 0 || spotLightData->SpotLightDir.z != 0) {
                glm::vec3 rotN = glm::cross(glm::vec3(1, 0, 0), spotLightData->SpotLightDir);
                float rotAngle = acos(glm::dot(glm::vec3(1, 0, 0), spotLightData->SpotLightDir));

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