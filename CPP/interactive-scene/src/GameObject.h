#pragma once
#ifndef GAMEOBJECT_H
#define GAMEOBJECT_H

#include <vector>
#include <Model.h>
#include <Transform.h>


class GameObject {
	public:
		GameObject* parent = nullptr;
		Transform* transform = nullptr;
		Model* model3d = nullptr;

		GameObject() {

		}

		GameObject(Transform* transform) : transform(transform) {

		}

		GameObject(Model* model3d, Shader* shader, Transform* transform) : model3d(model3d), shader(shader), transform(transform) {

		}

		~GameObject() {
			delete transform;

			GameObject* child;
			while (childs.size() > 0) {
				child = childs.back();
				childs.pop_back();
				delete child;
			}
		}

		Shader* getShader() {
			return shader;
		}

		Model* getModel() {
			return model3d;
		}

		Transform* getTransform() {
			return transform;
		}

		glm::vec3 getAbsolutePosition() {
			glm::vec3 absolutePosition = transform->getTranslate();

			GameObject* p = parent;

			while (p != nullptr) {
				absolutePosition += p->getTransform()->getTranslate();
				p = p->parent;
			}

			return absolutePosition;
		}

		glm::mat4 getAbsoluteTransformationMatrix() {
			glm::mat4 matrix = transform->getTransformMatrix();

			GameObject* p = parent;

			while (p != nullptr) {
				matrix = p->getTransform()->getTransformMatrix() * matrix;
				p = p->parent;
			}

			return matrix;
		}

		void setTransform(Transform* t) {
			delete transform;

			transform = t;
		}

		void addChild(GameObject* child) {
			childs.push_back(child);
			child->parent = this;
		}

		virtual void Update() {
			for (auto child : childs) {
				child->Update();
			}
		}

		virtual void Draw(glm::mat4& model) {
			glm::mat4 local = model * transform->getTransformMatrix();

			if (model3d != nullptr && shader != nullptr) {
				shader->use();
				shader->setMat4("model", local);
				model3d->Draw(shader);
			}

			for (auto child : childs) {
				child->Draw(local);
			}
		}

	private:
		std::vector<GameObject*> childs;
		Shader* shader = nullptr;
};

#endif