#version 430 core  
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 model;  
uniform mat4 view;  
uniform mat4 projection;

uniform vec4 inColor;
out vec4 fColor;
out vec2 TexCoords;

void main()  
{  
    gl_Position = projection * view * model * vec4(aPos, 1.0f); 
    fColor = inColor;
    TexCoords = aTexCoords;
}