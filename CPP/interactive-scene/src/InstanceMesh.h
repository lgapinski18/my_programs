#pragma once


struct InstancesData {
    int Amount = 0;
    vector<glm::vec3> Positions;
    vector<glm::vec3> Rotations;
    vector<glm::vec3> Scales;
    vector<bool> Usages;
    int UsedAmount = 0;
    glm::mat4* UsedModelMatrices;
};


class InstanceMesh : public Mesh {
    private:
        InstancesData* instanceData;
        unsigned int InstanceDataBuffer;

    public:
        InstanceMesh(vector<Vertex> vertices, vector<unsigned int> indices, vector<Texture> textures, InstancesData* instanceData) 
            : Mesh(vertices, indices, textures), instanceData(instanceData) {
            setUpInstancesData();
        }

        void Draw(Shader* shader) override
        {
            // bind appropriate textures
            unsigned int diffuseNr = 1;
            unsigned int specularNr = 1;
            unsigned int normalNr = 1;
            unsigned int heightNr = 1;
            for (unsigned int i = 0; i < textures.size(); i++)
            {
                glActiveTexture(GL_TEXTURE0 + i); // active proper texture unit before binding
                // retrieve texture number (the N in diffuse_textureN)
                string number;
                string name = textures[i].type;
                if (name == "texture_diffuse")
                    number = std::to_string(diffuseNr++);
                else if (name == "texture_specular")
                    number = std::to_string(specularNr++); // transfer unsigned int to string
                else if (name == "texture_normal")
                    number = std::to_string(normalNr++); // transfer unsigned int to string
                else if (name == "texture_height")
                    number = std::to_string(heightNr++); // transfer unsigned int to string

                // now set the sampler to the correct texture unit
                glUniform1i(glGetUniformLocation(shader->ID, (name + number).c_str()), i);
                // and finally bind the texture
                glBindTexture(GL_TEXTURE_2D, textures[i].id);
            }

            // draw mesh
            glBindVertexArray(VAO);
            //glDrawElements(GL_TRIANGLES, static_cast<unsigned int>(indices.size()), GL_UNSIGNED_INT, 0);
            //std::cout << instanceData->UsedAmount << std::endl;
            glDrawElementsInstanced(GL_TRIANGLES, static_cast<unsigned int>(indices.size()), GL_UNSIGNED_INT, 0, instanceData->UsedAmount);
            glBindVertexArray(0);

            // always good practice to set everything back to defaults once configured.
            glActiveTexture(GL_TEXTURE0);
        }

        void setUpInstancesData() {
            glGenBuffers(1, &InstanceDataBuffer);
            glBindBuffer(GL_ARRAY_BUFFER, InstanceDataBuffer);

            glBufferData(GL_ARRAY_BUFFER, instanceData->Amount * sizeof(glm::mat4), &(instanceData->UsedModelMatrices[0]), GL_STATIC_DRAW);

            glBindVertexArray(VAO);

            // set attribute pointers for matrix (4 times vec4)
            glEnableVertexAttribArray(3);
            glVertexAttribPointer(3, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)0);
            glEnableVertexAttribArray(4);
            glVertexAttribPointer(4, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(sizeof(glm::vec4)));
            glEnableVertexAttribArray(5);
            glVertexAttribPointer(5, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(2 * sizeof(glm::vec4)));
            glEnableVertexAttribArray(6);
            glVertexAttribPointer(6, 4, GL_FLOAT, GL_FALSE, sizeof(glm::mat4), (void*)(3 * sizeof(glm::vec4)));

            glVertexAttribDivisor(3, 1);
            glVertexAttribDivisor(4, 1);
            glVertexAttribDivisor(5, 1);
            glVertexAttribDivisor(6, 1);

            glBindVertexArray(0);
            /*for (unsigned int i = 0; i < rock.meshes.size(); i++)
            {
            }/**/
        }
};
