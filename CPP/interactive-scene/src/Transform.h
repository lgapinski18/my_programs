#pragma once

#ifndef TRANSFORM_H
#define TRANSFORM_H

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

class Transform {
	public:
		Transform() {

		}

		Transform(glm::vec3 translate, glm::vec3 rotation, glm::vec3 scale) : translate(translate), rotation(rotation), scale(scale) {

		}

		glm::vec3 getTranslate() {
			return translate;
		}

		glm::vec3 getRotation() {
			return rotation;
		}

		glm::vec3 getScale() {
			return scale;
		}

		void setTranslate(glm::vec3 vec) {
			translate = vec;
		}

		void setRotation(glm::vec3 vec) {
			rotation = vec;
		}

		void setRotationMatrix(glm::mat4 rotMatrix) {
			rotationMatrix = rotMatrix;
			useRotMat = true;
		}

		void setScale(glm::vec3 vec) {
			scale = vec;
		}

		glm::mat4 getTransformMatrix() {
			glm::mat4 transform = glm::mat4(1.0f);


			transform = glm::translate(transform, translate);

			if (!useRotMat) {
				transform = glm::rotate(transform, rotation.z, glm::vec3(0.0f, 0.0f, 1.0f));
				transform = glm::rotate(transform, rotation.y, glm::vec3(0.0f, 1.0f, 0.0f));
				transform = glm::rotate(transform, rotation.x, glm::vec3(1.0f, 0.0f, 0.0f));
			}
			else {
				transform *= rotationMatrix;
			}

			transform = glm::scale(transform, scale);

			return transform;
		}

	private:
		glm::vec3 translate	= glm::vec3(0.0f, 0.0f, 0.0f);
		glm::vec3 rotation	= glm::vec3(0.0f, 0.0f, 0.0f);
		glm::vec3 scale		= glm::vec3(1.0f, 1.0f, 1.0f);
		bool useRotMat = false;
		glm::mat4 rotationMatrix;
};

#endif // !TRANSFORM_H
