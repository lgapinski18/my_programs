configure_file(GameConfig.yaml.in GameConfig.yaml)

# ---- Dependency ----
add_subdirectory(Twin2Engine)

set(CMAKE_AUTORCC ON)

configure_file(${CMAKE_CURRENT_SOURCE_DIR}/icon.png ${CMAKE_CURRENT_BINARY_DIR}/icon.png COPYONLY)

add_executable(${PROJECT_NAME} main.cpp icon.rc)

target_precompile_headers(${PROJECT_NAME} PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}/pch.h)

add_subdirectory(Additional)

target_include_directories(${PROJECT_NAME} PUBLIC ${Twin2Engine_HEADER_DIR})
target_link_libraries(${PROJECT_NAME} PUBLIC Twin2Engine)

if (NOT CMAKE_BUILD_TYPE STREQUAL "Release")
    add_subdirectory(Editor)

    if (FIND_MEMORY_LEAKS)
        set(THREADS_PREFER_PTHREAD_FLAG ON)
        find_package(Threads REQUIRED)
        target_link_libraries(${PROJECT_NAME} PRIVATE Threads::Threads)

        target_link_libraries(${PROJECT_NAME} PRIVATE tracey_leak)
    endif()
    
else()
    target_compile_options(${PROJECT_NAME} PUBLIC "-O2")
endif()

target_link_libraries(${PROJECT_NAME} PUBLIC spdlog)
target_link_libraries(${PROJECT_NAME} PUBLIC spdlog::spdlog)

#add_custom_command(TARGET ${PROJECT_NAME} POST_BUILD 
#				   COMMAND ${CMAKE_COMMAND} -E create_symlink 
#				   ${CMAKE_SOURCE_DIR}/res 
#				   ${CMAKE_CURRENT_BINARY_DIR}/res)

file(CREATE_LINK ${CMAKE_SOURCE_DIR}/res ${CMAKE_CURRENT_BINARY_DIR}/res SYMBOLIC)

if(MSVC)
    target_compile_definitions(${PROJECT_NAME} PUBLIC NOMINMAX)
    target_compile_options(${PROJECT_NAME} PUBLIC "/Zc:preprocessor")
endif()

target_link_libraries(${PROJECT_NAME} PRIVATE Generation)
target_link_libraries(${PROJECT_NAME} PRIVATE GameScripts)

# Set the directory paths
#set(SHADERS_SOURCE_DIR "${CMAKE_SOURCE_DIR}/res/shaders/SPIRV")
#set(SHADERS_OUTPUT_DIR "${CMAKE_SOURCE_DIR}/res/CompiledShaders/SPIRV")

#find_program(GLSLANG_VALIDATOR NAMES glslangValidator)

# Add a target to compile all shaders
#add_custom_target(CompileShaders)

## Optionally, add the custom target to the default build target
#add_dependencies(Twin2Engine CompileShaders)
# Function to compile shaders using glslangValidator
#function(compile_shader shader_path)
#    get_filename_component(shader_name ${shader_path} NAME_WE)
#    get_filename_component(shader_ext ${shader_path} EXT)
#
#    file(RELATIVE_PATH RELATIVE_SOURCE ${SHADERS_SOURCE_DIR} ${shader_path})
#
#    #set(output_file "${SHADERS_OUTPUT_DIR}/${shader_name}${shader_ext}")
#    set(output_file "${SHADERS_OUTPUT_DIR}/${RELATIVE_SOURCE}")
#
#    # Create output directory
#
#    #file(MAKE_DIRECTORY ${SHADERS_OUTPUT_DIR})
#    
#    message("Shader to compile: ${shader_file}")
#    message("Shader compiled: ${output_file}\n")
#    # Execute glslangValidator to compile the shader
#    add_custom_command(
#        TARGET ${PROJECT_NAME} PRE_BUILD  
#        COMMAND ${CMAKE_CURRENT_SOURCE_DIR}/../tools/glslang/bin/glslangValidator.exe -G ${shader_path} -o ${output_file}
#        DEPENDS ${shader_path}
#        COMMENT "Compiling ${shader_name}.${shader_ext} to SPIR-V"
#        VERBATIM
#    )
#
#    # Return the compiled shader file path
#    set(${shader_path} ${output_file} PARENT_SCOPE)
#endfunction()

# Function to recursively find shader files
#function(find_shader_files dir)
#    file(GLOB_RECURSE shader_files LIST_DIRECTORIES false "${dir}/*.vert" "${dir}/*.frag" "${dir}/*.geom" "${dir}/*.tesc" "${dir}/*.tese" "${dir}/*.comp")
#    set(shader_files ${shader_files} PARENT_SCOPE)
#endfunction()

# Find all shader files
#find_shader_files(${SHADERS_SOURCE_DIR})

# Compile each shader file
#foreach(shader_file ${shader_files})
#    compile_shader(${shader_file})
#endforeach()

# Custom commend that will create _parameters.shpa files
#file(EXISTS ${SHADER_COMPILER_EXE} SHADER_COMPILER_EXE_EXISTS)
#if (${SHADER_COMPILER_EXE_EXISTS})
#if (EXISTS ${SHADER_COMPILER_EXE})
#    message("Additional prebuild program: ${SHADER_COMPILER_EXE}")
#    add_custom_command(
#        TARGET ${PROJECT_NAME} PRE_BUILD  
#        #COMMAND ${SHADER_COMPILER_EXE}
#        COMMENT "Running additional prebuild program: ${SHADER_COMPILER_EXE}"
#    )
#endif()