/*
 Navicat Premium Dump SQL

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 80041 (8.0.41)
 Source Host           : localhost:3306
 Source Schema         : ms

 Target Server Type    : MySQL
 Target Server Version : 80041 (8.0.41)
 File Encoding         : 65001

 Date: 02/06/2025 19:15:22
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for admin_information
-- ----------------------------
DROP TABLE IF EXISTS `admin_information`;
CREATE TABLE `admin_information`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '管理员id',
  `admin_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '管理员编号',
  `admin_password` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `admin_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '管理员姓名',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for course_information
-- ----------------------------
DROP TABLE IF EXISTS `course_information`;
CREATE TABLE `course_information`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '课程ID',
  `course_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程编号',
  `course_name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程名称',
  `course_description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '课程描述',
  `course_credit` decimal(3, 1) NOT NULL COMMENT '课程学分',
  `course_teacher_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程授课老师编号',
  `course_department` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程开课学院',
  `course_capacity` int NOT NULL DEFAULT 50 COMMENT '课程容量',
  `current_enrollment` int NOT NULL DEFAULT 0 COMMENT '当前选课人数',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `course_number`(`course_number` ASC) USING BTREE,
  INDEX `idx_course_teacher`(`course_teacher_number` ASC) USING BTREE,
  INDEX `idx_course_department`(`course_department` ASC) USING BTREE,
  INDEX `idx_capacity`(`course_capacity` ASC, `current_enrollment` ASC) USING BTREE,
  CONSTRAINT `course_information_ibfk_1` FOREIGN KEY (`course_teacher_number`) REFERENCES `teacher_information` (`teacher_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `chk_credit_range` CHECK (`course_credit` between 1 and 5)
) ENGINE = InnoDB AUTO_INCREMENT = 24 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '课程信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for course_selection
-- ----------------------------
DROP TABLE IF EXISTS `course_selection`;
CREATE TABLE `course_selection`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '选课ID',
  `student_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '学号',
  `course_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程编号',
  `teacher_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '授课老师编号',
  `course_status` enum('未选','已选','退选') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '未选' COMMENT '选课状态（如“已选”、“退选”、”未选“）',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_selection_combo`(`student_number` ASC, `course_number` ASC) USING BTREE,
  INDEX `teacher_number`(`teacher_number` ASC) USING BTREE,
  INDEX `idx_course_status`(`course_number` ASC, `course_status` ASC) USING BTREE,
  CONSTRAINT `course_selection_ibfk_1` FOREIGN KEY (`student_number`) REFERENCES `student_information` (`student_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `course_selection_ibfk_2` FOREIGN KEY (`course_number`) REFERENCES `course_information` (`course_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `course_selection_ibfk_3` FOREIGN KEY (`teacher_number`) REFERENCES `teacher_information` (`teacher_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `chk_course_status` CHECK (`course_status` in (_utf8mb4'已选',_utf8mb4'退选'))
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '选课信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for course_tables
-- ----------------------------
DROP TABLE IF EXISTS `course_tables`;
CREATE TABLE `course_tables`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '课程id',
  `course_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程编号',
  `course_type` enum('必修','选修','实践','讲座') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '课程类型',
  `course_teacher_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '授课老师编号',
  `course_location` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '上课地点',
  `course_start_time` time NOT NULL,
  `course_end_time` time NOT NULL,
  `course_day_of_week` enum('1','2','3','4','5','6','7') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '星期几',
  `course_week_pattern` enum('每周','单周','双周','指定周') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '周模式',
  `course_specific_weeks` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '指定周数',
  `course_start_date` date NULL DEFAULT NULL COMMENT '课程开始日期',
  `course_end_date` date NULL DEFAULT NULL COMMENT '课程结束日期',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `course_teacher_number`(`course_teacher_number` ASC) USING BTREE,
  INDEX `course_number`(`course_number` ASC) USING BTREE,
  CONSTRAINT `course_tables_ibfk_1` FOREIGN KEY (`course_teacher_number`) REFERENCES `teacher_information` (`teacher_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `course_tables_ibfk_2` FOREIGN KEY (`course_number`) REFERENCES `course_information` (`course_number`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 34 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for student_grades
-- ----------------------------
DROP TABLE IF EXISTS `student_grades`;
CREATE TABLE `student_grades`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '成绩ID',
  `student_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '学号',
  `course_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '课程编号',
  `semester` enum('2020-2021学年','2022-2023学年','2024-2025学年','2026-2027学年','2028-2029学年') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '2020-2021学年' COMMENT '学期',
  `exam_type` enum('小测','期中','期末') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '小测' COMMENT '考试类型（如“期中”、“期末”、“小测”）',
  `score` decimal(5, 2) NULL DEFAULT NULL COMMENT '成绩',
  `grade_point` decimal(3, 2) NULL DEFAULT NULL COMMENT '绩点',
  `credit` decimal(3, 1) NULL DEFAULT NULL COMMENT '学分',
  `teacher_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '授课老师编号',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `idx_grade_student`(`student_number` ASC) USING BTREE,
  INDEX `idx_grade_course`(`course_number` ASC) USING BTREE,
  INDEX `idx_grade_term`(`semester` ASC) USING BTREE,
  CONSTRAINT `student_grades_ibfk_1` FOREIGN KEY (`student_number`) REFERENCES `student_information` (`student_number`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `student_grades_ibfk_2` FOREIGN KEY (`course_number`) REFERENCES `course_information` (`course_number`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 189 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '学生成绩表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for student_information
-- ----------------------------
DROP TABLE IF EXISTS `student_information`;
CREATE TABLE `student_information`  (
  `id` int(10) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT COMMENT '学生ID',
  `student_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '学号，唯一',
  `student_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '学生姓名',
  `student_password` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `student_gender` enum('男','女') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '男' COMMENT '性别',
  `student_class` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '班级',
  `student_major` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '专业',
  `student_department` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '学院',
  `student_birthday` date NULL DEFAULT NULL COMMENT '出生日期',
  `student_origin` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '籍贯',
  `student_address` varchar(100) CHARACTER SET utf16le COLLATE utf16le_general_ci NULL DEFAULT NULL COMMENT '地址',
  `student_phone` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '电话号码，固定长度11位',
  `student_email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '邮箱',
  `student_enrollment_date` date NULL DEFAULT NULL COMMENT '入学日期',
  `student_graduation_date` date NULL DEFAULT NULL COMMENT '毕业日期',
  `total_credits` decimal(5, 1) UNSIGNED NOT NULL DEFAULT 0.0 COMMENT '累计学分',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `student_number`(`student_number` ASC) USING BTREE,
  INDEX `idx_student_number`(`student_number` ASC) USING BTREE,
  INDEX `idx_student_class`(`student_class` ASC) USING BTREE,
  CONSTRAINT `chk_student_email` CHECK (`student_email` like _utf8mb4'%@%.%')
) ENGINE = InnoDB AUTO_INCREMENT = 29 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '学生信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for teacher_information
-- ----------------------------
DROP TABLE IF EXISTS `teacher_information`;
CREATE TABLE `teacher_information`  (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '教师ID',
  `teacher_number` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '工号，唯一',
  `teacher_name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '教师姓名',
  `teacher_password` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `teacher_gender` enum('男','女') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '男' COMMENT '性别',
  `teacher_title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '职称',
  `teacher_department` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '学院',
  `teacher_birthday` date NULL DEFAULT NULL COMMENT '出生日期',
  `teacher_phone` varchar(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '电话号码，固定长度11位',
  `teacher_email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '邮箱',
  `teacher_professional_field` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '专业领域',
  `teacher_education_level` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '学历',
  `teacher_work_start_date` date NULL DEFAULT NULL COMMENT '工作开始日期',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `teacher_number`(`teacher_number` ASC) USING BTREE,
  INDEX `idx_teacher_department`(`teacher_department` ASC) USING BTREE,
  CONSTRAINT `chk_teacher_email` CHECK (`teacher_email` like _utf8mb4'%@%.%')
) ENGINE = InnoDB AUTO_INCREMENT = 39 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '教师信息表' ROW_FORMAT = Dynamic;

-- ----------------------------
-- View structure for current_week_view
-- ----------------------------
DROP VIEW IF EXISTS `current_week_view`;
CREATE ALGORITHM = UNDEFINED SQL SECURITY DEFINER VIEW `current_week_view` AS select `course_tables`.`id` AS `id`,`course_tables`.`course_number` AS `course_number`,`course_tables`.`course_type` AS `course_type`,`course_tables`.`course_teacher_number` AS `course_teacher_number`,`course_tables`.`course_location` AS `course_location`,`course_tables`.`course_start_time` AS `course_start_time`,`course_tables`.`course_end_time` AS `course_end_time`,`course_tables`.`course_day_of_week` AS `course_day_of_week`,`course_tables`.`course_week_pattern` AS `course_week_pattern`,`course_tables`.`course_specific_weeks` AS `course_specific_weeks`,`course_tables`.`course_start_date` AS `course_start_date`,`course_tables`.`course_end_date` AS `course_end_date` from `course_tables` where ((curdate() between `course_tables`.`course_start_date` and `course_tables`.`course_end_date`) and ((`course_tables`.`course_week_pattern` = '每周') or ((`course_tables`.`course_week_pattern` = '单周') and ((week(curdate(),1) % 2) = 1)) or ((`course_tables`.`course_week_pattern` = '双周') and ((week(curdate(),1) % 2) = 0)) or ((`course_tables`.`course_week_pattern` = '指定周') and (find_in_set(((week(curdate(),1) - week(`course_tables`.`course_start_date`,1)) + 1),`NormalizeWeekNumbers`(`course_tables`.`course_specific_weeks`)) > 0)))) order by `course_tables`.`course_day_of_week`,`course_tables`.`course_start_time`;

-- ----------------------------
-- View structure for next_week_view
-- ----------------------------
DROP VIEW IF EXISTS `next_week_view`;
CREATE ALGORITHM = UNDEFINED SQL SECURITY DEFINER VIEW `next_week_view` AS select `course_tables`.`id` AS `id`,`course_tables`.`course_number` AS `course_number`,`course_tables`.`course_type` AS `course_type`,`course_tables`.`course_teacher_number` AS `course_teacher_number`,`course_tables`.`course_location` AS `course_location`,`course_tables`.`course_start_time` AS `course_start_time`,`course_tables`.`course_end_time` AS `course_end_time`,`course_tables`.`course_day_of_week` AS `course_day_of_week`,`course_tables`.`course_week_pattern` AS `course_week_pattern`,`course_tables`.`course_specific_weeks` AS `course_specific_weeks`,`course_tables`.`course_start_date` AS `course_start_date`,`course_tables`.`course_end_date` AS `course_end_date` from `course_tables` where (((curdate() + interval 7 day) between `course_tables`.`course_start_date` and `course_tables`.`course_end_date`) and ((`course_tables`.`course_week_pattern` = '每周') or ((`course_tables`.`course_week_pattern` = '单周') and ((week((curdate() + interval 7 day),1) % 2) = 1)) or ((`course_tables`.`course_week_pattern` = '双周') and ((week((curdate() + interval 7 day),1) % 2) = 0)) or ((`course_tables`.`course_week_pattern` = '指定周') and (find_in_set(((week((curdate() + interval 7 day),1) - week(`course_tables`.`course_start_date`,1)) + 1),`NormalizeWeekNumbers`(`course_tables`.`course_specific_weeks`)) > 0)))) order by `course_tables`.`course_day_of_week`,`course_tables`.`course_start_time`;

-- ----------------------------
-- Function structure for isValidPhone
-- ----------------------------
DROP FUNCTION IF EXISTS `isValidPhone`;
delimiter ;;
CREATE FUNCTION `isValidPhone`(p VARCHAR(11))
 RETURNS tinyint(1)
  DETERMINISTIC
BEGIN
    RETURN (p REGEXP '^[0-9]{11}$');
END
;;
delimiter ;

-- ----------------------------
-- Function structure for NormalizeWeekNumbers
-- ----------------------------
DROP FUNCTION IF EXISTS `NormalizeWeekNumbers`;
delimiter ;;
CREATE FUNCTION `NormalizeWeekNumbers`(weeks VARCHAR(50))
 RETURNS varchar(100) CHARSET utf8mb4
  DETERMINISTIC
BEGIN
    DECLARE result VARCHAR(100) DEFAULT '';
    DECLARE part VARCHAR(10);
    DECLARE start_range INT;
    DECLARE end_range INT;
    
    -- 空值处理
    IF weeks IS NULL OR weeks = '' THEN
        RETURN '';
    END IF;
    
    -- 移除空格并分割字符串
    SET weeks = REPLACE(weeks, ' ', '');
    
    WHILE LENGTH(weeks) > 0 DO
        -- 获取第一个分段
        SET part = SUBSTRING_INDEX(weeks, ',', 1);
        SET weeks = SUBSTRING(weeks, LENGTH(part) + 2);
        
        -- 处理范围表达式
        IF INSTR(part, '-') > 0 THEN
            SET start_range = CAST(SUBSTRING_INDEX(part, '-', 1) AS UNSIGNED);
            SET end_range = CAST(SUBSTRING_INDEX(part, '-', -1) AS UNSIGNED);
            
            WHILE start_range <= end_range DO
                SET result = CONCAT_WS(',', result, start_range);
                SET start_range = start_range + 1;
            END WHILE;
        ELSE
            SET result = CONCAT_WS(',', result, part);
        END IF;
    END WHILE;
    
    RETURN TRIM(BOTH ',' FROM result);
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for update_course_status
-- ----------------------------
DROP PROCEDURE IF EXISTS `update_course_status`;
delimiter ;;
CREATE PROCEDURE `update_course_status`(IN p_student_num VARCHAR(20),
    IN p_course_num VARCHAR(20),
    IN p_new_status VARCHAR(20))
BEGIN
    DECLARE old_status VARCHAR(20);
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    START TRANSACTION;

    -- 获取原状态（带行锁）
    SELECT course_status INTO old_status
    FROM course_selection
    WHERE student_number = p_student_num
      AND course_number = p_course_num
    FOR UPDATE;

    -- 更新状态
    UPDATE course_selection
    SET course_status = p_new_status
    WHERE student_number = p_student_num
      AND course_number = p_course_num;

    -- 维护选课人数
    IF old_status != '退选' AND p_new_status = '退选' THEN
        UPDATE course_information
        SET current_enrollment = current_enrollment - 1
        WHERE course_number = p_course_num;
    ELSEIF old_status = '退选' AND p_new_status != '退选' THEN
        UPDATE course_information
        SET current_enrollment = current_enrollment + 1
        WHERE course_number = p_course_num;
    END IF;

    COMMIT;
END
;;
delimiter ;

-- ----------------------------
-- Function structure for ValidateWeekNumber
-- ----------------------------
DROP FUNCTION IF EXISTS `ValidateWeekNumber`;
delimiter ;;
CREATE FUNCTION `ValidateWeekNumber`(check_date DATE, 
    start_date DATE, 
    specific_weeks VARCHAR(50))
 RETURNS tinyint(1)
  DETERMINISTIC
BEGIN
    DECLARE current_week INT;
    DECLARE start_week INT;
    DECLARE relative_week INT;
    DECLARE normalized_weeks VARCHAR(100);
    
    -- 计算当前周（周一作为周开始）
    SET current_week = WEEK(check_date, 1);
    SET start_week = WEEK(start_date, 1);
    SET relative_week = current_week - start_week + 1;
    
    -- 标准化周数格式
    SET normalized_weeks = NormalizeWeekNumbers(specific_weeks);
    
    RETURN FIND_IN_SET(relative_week, normalized_weeks) > 0;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table course_selection
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_schedule_conflict`;
delimiter ;;
CREATE TRIGGER `trg_schedule_conflict` BEFORE INSERT ON `course_selection` FOR EACH ROW BEGIN
    DECLARE v_conflict_count INT DEFAULT 0;
    
    -- 检查是否存在时间重叠
    SELECT COUNT(*) INTO v_conflict_count
    FROM (
        -- 获取学生已选课程的时间安排
        SELECT ct.course_day_of_week, 
               ct.course_week_pattern,
               ct.course_start_time,
               ct.course_end_time
        FROM course_selection cs
        JOIN course_tables ct ON cs.course_number = ct.course_number
        WHERE cs.student_number = NEW.student_number
          AND cs.course_status != '退选'
    ) AS existing_courses
    JOIN (
        -- 获取新选课程的时间安排
        SELECT course_day_of_week,
               course_week_pattern,
               course_start_time,
               course_end_time
        FROM course_tables
        WHERE course_number = NEW.course_number
    ) AS new_course
    ON existing_courses.course_day_of_week = new_course.course_day_of_week
    WHERE (
        -- 周模式匹配规则
        existing_courses.course_week_pattern = '每周' OR
        new_course.course_week_pattern = '每周' OR
        existing_courses.course_week_pattern = new_course.course_week_pattern
    )
    AND (
        -- 时间重叠判断条件
        (new_course.course_start_time < existing_courses.course_end_time) AND
        (new_course.course_end_time > existing_courses.course_start_time)
    );
    
    IF v_conflict_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = '选课失败：存在时间冲突';
    END IF;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table course_selection
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_course_capacity_check`;
delimiter ;;
CREATE TRIGGER `trg_course_capacity_check` BEFORE INSERT ON `course_selection` FOR EACH ROW BEGIN
          DECLARE
          v_max_cap INT;
          DECLARE
          v_curr_enroll INT;
          DECLARE
          v_error_msg VARCHAR (255);
          -- 获取实时数据（带共享锁）
          SELECT
            course_capacity,
            current_enrollment INTO v_max_cap,
            v_curr_enroll
          FROM
            course_information
          WHERE
            course_number = NEW.course_number LOCK IN SHARE MODE;
          -- 人数检查（排除退选）
          IF NEW.course_status != '退选'
            AND v_curr_enroll >= v_max_cap THEN
            SET v_error_msg = CONCAT('课程已满');
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = v_error_msg;
          END IF;
        END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table course_selection
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_update_enrollment`;
delimiter ;;
CREATE TRIGGER `trg_update_enrollment` AFTER INSERT ON `course_selection` FOR EACH ROW BEGIN
            IF NEW.course_status != '退选' THEN
              UPDATE course_information
              SET current_enrollment = current_enrollment + 1
              WHERE
                course_number = NEW.course_number;
            END IF;
          END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table course_selection
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_update_enrollment_on_status_change`;
delimiter ;;
CREATE TRIGGER `trg_update_enrollment_on_status_change` AFTER UPDATE ON `course_selection` FOR EACH ROW BEGIN
              -- 退选 -> 其他状态
              IF OLD.course_status = '退选'
                AND NEW.course_status != '退选' THEN
                UPDATE course_information
                SET current_enrollment = current_enrollment + 1
                WHERE
                  course_number = NEW.course_number;
                -- 其他状态 -> 退选
              ELSEIF OLD.course_status != '退选'
                AND NEW.course_status = '退选' THEN
                UPDATE course_information
                SET current_enrollment = current_enrollment - 1
                WHERE
                  course_number = OLD.course_number;
              END IF;
            END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table course_tables
-- ----------------------------
DROP TRIGGER IF EXISTS `check_course_time`;
delimiter ;;
CREATE TRIGGER `check_course_time` BEFORE INSERT ON `course_tables` FOR EACH ROW BEGIN
    -- 检查上课时间是否早于下课时间
    IF NEW.course_start_time >= NEW.course_end_time THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = '课程开始时间必须早于结束时间';
    END IF;

    -- 检查课程日期范围是否合理
    IF NEW.course_start_date > NEW.course_end_date THEN
        SIGNAL SQLSTATE '45001' 
        SET MESSAGE_TEXT = '课程开始日期不能晚于结束日期';
    END IF;
    
    -- 当选择指定周模式时必须填写具体周数
    IF NEW.course_week_pattern = '指定周' AND 
       (NEW.course_specific_weeks IS NULL OR NEW.course_specific_weeks = '') THEN
        SIGNAL SQLSTATE '45002'
        SET MESSAGE_TEXT = '指定周模式必须设置具体周数';
    END IF;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table student_information
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_validate_phone_insert`;
delimiter ;;
CREATE TRIGGER `trg_validate_phone_insert` BEFORE INSERT ON `student_information` FOR EACH ROW BEGIN
    IF NOT isValidPhone(NEW.student_phone) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Invalid phone format (必须为11位纯数字)';
    END IF;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table student_information
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_validate_phone_update`;
delimiter ;;
CREATE TRIGGER `trg_validate_phone_update` BEFORE UPDATE ON `student_information` FOR EACH ROW BEGIN
    IF NOT isValidPhone(NEW.student_phone) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Invalid phone format (必须为11位纯数字)';
    END IF;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table teacher_information
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_validate_teacher_phone_insert`;
delimiter ;;
CREATE TRIGGER `trg_validate_teacher_phone_insert` BEFORE INSERT ON `teacher_information` FOR EACH ROW BEGIN
    -- 允许空值时可添加 NULL 判断
    IF NEW.teacher_phone IS NOT NULL 
       AND NOT isValidPhone(NEW.teacher_phone) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = '教师手机号格式错误：必须为11位纯数字';
    END IF;
END
;;
delimiter ;

-- ----------------------------
-- Triggers structure for table teacher_information
-- ----------------------------
DROP TRIGGER IF EXISTS `trg_validate_teacher_phone_update`;
delimiter ;;
CREATE TRIGGER `trg_validate_teacher_phone_update` BEFORE UPDATE ON `teacher_information` FOR EACH ROW BEGIN
    IF NEW.teacher_phone IS NOT NULL 
       AND NOT isValidPhone(NEW.teacher_phone) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = '教师手机号格式错误：必须为11位纯数字';
    END IF;
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
