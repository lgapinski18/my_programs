#version 430 core
out vec4 color;

in vec4 inColor;
in vec4 fColor;
in vec2 TexCoords;

uniform sampler2D texture_diffuse1;

void main()
{    
    color = fColor;
    //color = texture(texture_diffuse1, TexCoords) * fColor;
}