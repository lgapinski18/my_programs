#pragma once

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>


struct PointLightData {
    bool PointLightOn;
    glm::vec4 PointLightColor;
    glm::vec3 PointLightPosition;
    float PointLightPower;
    float PointLightRadius;
};

class PointLight : public GameObject {
private:
    PointLightData* pointLightData;
    Shader* shader1;
    Shader* shader2;
    float CX = 0;
    float CZ = 0;
    float Radius = 0;
    float angle = 0;

public:
    PointLight(Model* model3d, Shader* shader, Transform* transform, PointLightData* pointLightData, 
                float CX, float CZ, float Radius, Shader* shader1, Shader* shader2)
        : GameObject(model3d, shader, transform), pointLightData(pointLightData), 
            CX(CX), CZ(CZ), Radius(Radius), shader1(shader1), shader2(shader2) {
    }

    PointLightData* getLightData() {
        return pointLightData;
    }

    void Update() override {
        pointLightData->PointLightPosition = glm::vec3(cos(angle) * Radius + CX, 
            pointLightData->PointLightPosition.y, sin(angle) * Radius + CZ);

        getTransform()->setTranslate(pointLightData->PointLightPosition); // / getTransform()->getScale().x

        shader1->use();
        shader1->setVec3("PointLightPosition", pointLightData->PointLightPosition);

        shader2->use();
        shader2->setVec3("PointLightPosition", pointLightData->PointLightPosition);

        angle += 0.01;

        if (angle > 6.28) {
            angle = 0;
        }

        GameObject::Update();
    }

    void Draw(glm::mat4& model) override {

        if (!pointLightData->PointLightOn) {
            return;
        }

        getShader()->use();
        getShader()->setVec4("inColor",
            glm::vec4(pointLightData->PointLightColor.x, pointLightData->PointLightColor.y, pointLightData->PointLightColor.z, 1));

        Transform* t = new Transform();

        t->setTranslate(getTransform()->getTranslate());
        t->setScale(glm::vec3(pointLightData->PointLightRadius, pointLightData->PointLightRadius, pointLightData->PointLightRadius));
        glm::mat4 local = model * t->getTransformMatrix();

        if (getModel() != nullptr) {
            glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            getShader()->setMat4("model", local);
            getModel()->Draw(getShader());
            glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
        }

        delete t;

        GameObject::Draw(model);
    }

    void UpdateLightData() {
        shader1->use();

        shader1->setBool("PointLightOn", pointLightData->PointLightOn);
        shader1->setVec4("PointLightColor", pointLightData->PointLightColor);
        shader1->setVec3("PointLightPosition", pointLightData->PointLightPosition);
        shader1->setFloat("PointLightPower", pointLightData->PointLightPower);
        shader1->setFloat("PointLightRadius", pointLightData->PointLightRadius);

        shader2->use();

        shader2->setBool("PointLightOn", pointLightData->PointLightOn);
        shader2->setVec4("PointLightColor", pointLightData->PointLightColor);
        shader2->setVec3("PointLightPosition", pointLightData->PointLightPosition);
        shader2->setFloat("PointLightPower", pointLightData->PointLightPower);
        shader2->setFloat("PointLightRadius", pointLightData->PointLightRadius);
    }
};